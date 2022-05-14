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
    /// 聊天Hub
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
        /// 用户加入到角色聊天组
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task JoinRoleGroup(string roleId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, roleId);
            await Clients.Group(roleId).SendAsync("Receive", $"{_currentUser.UserName} joined {roleId}");
        }

        /// <summary>
        /// 给角色聊天组发音频消息
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
                        _logger.LogError($"持久化发送消息异常,信息如下:{ex.Message},堆栈:{ex.StackTrace}");
                        await uow.RollbackAsync();
                    }
                }
            }

            //生成音频文件
            var audioFilePath=await _audioMessageHelper.CreateAsync(message, Context.GetHttpContext().RequestAborted);

            //通过ws发送消息到客户端发送到客户端
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
        /// 向用户发送消息 支持发送消息的同时指定是否需要用户确认
        /// </summary>
        /// <param name="userId">用户id</param>
        /// <param name="message">消息</param>
        /// <param name="needUserConfirmed">是否需要用户确认</param>
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

                        //生成音频文件
                        var audioFilePath = await _audioMessageHelper.CreateAsync(message, Context.GetHttpContext().RequestAborted);

                        //通过ws发送消息到客户端发送到客户端
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
                        _logger.LogError($"持久化发送消息异常,信息如下:{ex.Message},堆栈:{ex.StackTrace}");
                        await uow.RollbackAsync();
                    }
                }
            }
        }

        /// <summary>
        /// 用户消息确认
        /// </summary>
        /// <param name="messageId">消息id</param>
        /// <param name="senderId">发送者Id</param>
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
                        var backMessage = $"{_currentUser.UserName}已确认消息";

                        //生成音频文件
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
                        _logger.LogError($"持久化发送消息异常,信息如下:{ex.Message},堆栈:{ex.StackTrace}");
                        await uow.RollbackAsync();
                    }
                }
            }
        }
    }
}
