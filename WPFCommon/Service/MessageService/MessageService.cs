using System.Linq;
using static WPFCommon.IMessageService;

namespace WPFCommon
{
    /// <summary>
    /// メッセージマスタからメッセージを取得するクラス
    /// </summary>
    public class MessageService : IMessageService
    {
        private static readonly SQLStrings _SQLString = new();

        private readonly IDatabaseService _Conn = null;

        static MessageService()
        {
            _SQLString.AppendLine("SELECT");
            _SQLString.AppendLine("    MESSAGE_TYPE AS MessageType,");
            _SQLString.AppendLine("    MESSAGE AS MessageString");
            _SQLString.AppendLine("FROM");
            _SQLString.AppendLine("    M_MESSAGE");
            _SQLString.AppendLine("WHERE");
            _SQLString.AppendLine("    MSG_ID = :MSG_ID");
            _SQLString.AppendLine("AND LANG_CD = :LANG_CD");
        }

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        /// <param name="msgID">メッセージID</param>
        /// <param name="langCD">言語コード</param>
        /// <param name="addStrings">メッセージ内容置換文字配列</param>
        public MessageService(IDatabaseService iDatabase)
        {
            this._Conn = iDatabase;
        }

        /// <summary>
        /// クラスのインスタンス作成時に実行されるコンストラクタ
        /// </summary>
        private MessageService()
        {
        }

        public MessageInfo Get(string msgID, string langCD, params string[] addStrings)
        {
            var param = new
            {
                MSG_ID = msgID,
                LANG_CD = langCD
            };

            var ret = this._Conn.Select<MessageInfo>(_SQLString, param);

            if (ret.Count == 0)
            {
                var defaultRecord = this._Conn.Select<MessageInfo>(_SQLString, new { MSG_ID = "99999", LANG_CD = langCD });
                var first = defaultRecord.FirstOrDefault();
                first.MessageString = string.Format(first.MessageString, msgID);
                return first;
            }
            else
            {
                var first = ret.FirstOrDefault();
                if (addStrings.IsEmpty() == false)
                {
                    first.MessageString = string.Format(first.MessageString, addStrings);
                }
                return first;
            }
        }
    }
}