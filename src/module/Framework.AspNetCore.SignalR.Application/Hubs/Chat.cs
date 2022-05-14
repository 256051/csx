using Framework.AspNetCore.SignalR.Application.Hubs;
using Framework.AspNetCore.SignalR.Application.Models;
using Framework.AspNetCore.SignalR.Application.Services;
using Framework.IM.Message.Domain.Shared;
using Framework.Uow;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Framework.AspNetCore.SignalR.Application
{
    /// <summary>
    /// ����Hub
    /// </summary>
    public class Chat : HubBase
    {
        private ILogger<Chat> _logger;
        private AudioMessageHelper _audioMessageHelper;
        public Chat(ILogger<Chat> logger, AudioMessageHelper audioMessageHelper)
        {
            _logger = logger;
            _audioMessageHelper = audioMessageHelper;
        }

        /// <summary>
        /// �û����뵽��ɫ������
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task JoinRoleGroup(string roleId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roleId);
            await Clients.Group(roleId).SendAsync("Receive", $"{_currentUser.UserName} joined {roleId}");
        }

        /// <summary>
        /// ����ɫ�����鷢��Ƶ��Ϣ
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task SendToRoleGroup(string roleId,string message)
        {
            var messages = Enumerable.Empty<ImUserMessage>();
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions() { IsTransactional = true }))
                {
                    var _userMessageManager = scope.ServiceProvider.GetRequiredService<ApplicationUserMessageManager>();

                    var _userProvider = scope.ServiceProvider.GetRequiredService<IUserInfoProvider>();

                    var receivers = await _userProvider.FindUsersByRoleIdAsync(roleId, Context.GetHttpContext().RequestAborted);

                    messages = receivers.Select(receiver => new ImUserMessage()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SenderId = _currentUser.Id,
                        ReceiverId = receiver.UserId,
                        Content = message,
                        CreateTime = DateTime.Now
                    });
                    try
                    {
                        await _userMessageManager.CreateMessagesAsync(messages);
                        await uow.CompleteAsync();
                    }
                    catch(Exception ex)
                    {
                        _logger.LogError($"�־û�������Ϣ�쳣,��Ϣ����:{ex.Message},��ջ:{ex.StackTrace}");
                        await uow.RollbackAsync();
                    }
                }
            }

            //������Ƶ�ļ�
            var audioFilePath=await _audioMessageHelper.CreateAsync(message, Context.GetHttpContext().RequestAborted);

            //ͨ��ws������Ϣ���ͻ��˷��͵��ͻ���
            foreach (var immessage in messages)
            {
                await Clients.User(immessage.ReceiverId).SendAsync("Receive", _jsonSerializer.Serialize(new MediaMessageResponse() { 
                    MessageId= immessage.Id,
                    MediaFilePath= audioFilePath,
                    Message=message
                }));
            }
        }

        /// <summary>
        /// ���û�������Ϣ ֧�ַ�����Ϣ��ͬʱָ���Ƿ���Ҫ�û�ȷ��
        /// </summary>
        /// <param name="userId">�û�id</param>
        /// <param name="message">��Ϣ</param>
        /// <param name="needUserConfirmed">�Ƿ���Ҫ�û�ȷ��</param>
        /// <returns></returns>
        public async Task SendToUser(string userId, string message,bool? needUserConfirmed)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions() { IsTransactional = true }))
                {
                    var _userMessageManager = scope.ServiceProvider.GetRequiredService<ApplicationUserMessageManager>();

                    var userMessage = new ImUserMessage()
                    {
                        Id = Guid.NewGuid().ToString(),
                        SenderId = _currentUser.Id,
                        ReceiverId = userId,
                        Content = message,
                        CreateTime = DateTime.Now
                    };

                    if (needUserConfirmed.HasValue && needUserConfirmed.Value)
                    {
                        userMessage.Confirmed = false;
                    }

                    try
                    {
                        await _userMessageManager.CreateMessageAsync(userMessage);

                        //������Ƶ�ļ�
                        var audioFilePath = await _audioMessageHelper.CreateAsync(message, Context.GetHttpContext().RequestAborted);

                        //ͨ��ws������Ϣ���ͻ��˷��͵��ͻ���
                        await Clients.User(userId).SendAsync("Receive", _jsonSerializer.Serialize(new MediaMessageResponse()
                        {
                            MessageId = userMessage.Id,
                            MediaFilePath = audioFilePath,
                            Message = message,
                            SendTime= userMessage.CreateTime
                        }));
                        await uow.CompleteAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"�־û�������Ϣ�쳣,��Ϣ����:{ex.Message},��ջ:{ex.StackTrace}");
                        await uow.RollbackAsync();
                    }
                }
            }
        }

        /// <summary>
        /// �û���Ϣȷ��
        /// </summary>
        /// <param name="messageId">��Ϣid</param>
        /// <param name="senderId">������Id</param>
        /// <returns></returns>
        public async Task MessageConfirm(string messageId)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                using (var uow = _unitOfWorkManager.Begin(new UnitOfWorkOptions() { IsTransactional = true }))
                {
                    var _userMessageManager = scope.ServiceProvider.GetRequiredService<ApplicationUserMessageManager>();
                    try
                    {
                        await _userMessageManager.ConfirmMessageByIdAsync(messageId);
                        var message =await _userMessageManager.FindByIdAsync(messageId);
                        var backMessage = $"{_currentUser.UserName}��ȷ����Ϣ";

                        //������Ƶ�ļ�
                        var audioFilePath = await _audioMessageHelper.CreateAsync(backMessage, Context.GetHttpContext().RequestAborted);
                        await Clients.User(message.SenderId).SendAsync("Receive", _jsonSerializer.Serialize(new MediaMessageResponse() { 
                            MediaFilePath= audioFilePath,
                            Message= message.Content,
                            SenderName= _currentUser.UserName.Trim().Replace(" ",""),
                            SendTime= message.CreateTime,
                            ConfirmedTime= message.ConfirmedTime
                        }));;
                        await uow.CompleteAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"�־û�������Ϣ�쳣,��Ϣ����:{ex.Message},��ջ:{ex.StackTrace}");
                        await uow.RollbackAsync();
                    }
                }
            }
        }
    }
}
