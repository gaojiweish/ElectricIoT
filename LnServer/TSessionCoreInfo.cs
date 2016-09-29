using System;

namespace LnServer
{
    public class TSessionCoreInfo
    {
        #region  member fields

        private int m_id;
        private string m_ip = string.Empty;
        private string m_name = string.Empty;
        private TSessionState m_state = TSessionState.Active;
        private TDisconnectType m_disconnectType = TDisconnectType.Normal;

        private DateTime m_loginTime;
        private DateTime m_lastSessionTime;

        #endregion

        #region  public properties

        public int ID
        {
            get { return m_id; }
            protected set { m_id = value; }
        }

        public string IP
        {
            get { return m_ip; }
            protected set { m_ip = value; }
        }

        /// <summary>
        /// 数据包发送者的名称/编号
        /// </summary>
        public string Name
        {
            get { return m_name; }
            protected set { m_name = value; }
        }

        public DateTime LoginTime
        {
            get { return m_loginTime; }
            protected set
            {
                m_loginTime = value;
                m_lastSessionTime = value;
            }
        }

        public DateTime LastSessionTime
        {
            get { return m_lastSessionTime; }
            protected set { m_lastSessionTime = value; }
        }

        public TSessionState State
        {
            get { return m_state; }
            protected set
            {
                lock (this)
                {
                    m_state = value;
                }
            }
        }

        public TDisconnectType DisconnectType
        {
            get { return m_disconnectType; }
            protected set
            {
                lock (this)
                {
                    m_disconnectType = value;
                }
            }
        }
        #endregion
    }
}
