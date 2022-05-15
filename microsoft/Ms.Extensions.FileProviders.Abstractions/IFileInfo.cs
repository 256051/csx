using System;
using System.IO;

namespace Ms.Extensions.FileProviders.Abstractions
{
    public interface IFileInfo
    {
        /// <summary>
        /// �ļ��Ƿ����
        /// </summary>
        bool Exists { get; }

        /// <summary>
        /// �ļ���·���������ļ���������ļ�����ֱ�ӷ��ʣ��򷵻�null��
        /// </summary>
        string PhysicalPath { get; }

        /// <summary>
        /// ��ȡ�ļ���
        /// </summary>
        /// <returns></returns>
        Stream CreateReadStream();
    }
}
