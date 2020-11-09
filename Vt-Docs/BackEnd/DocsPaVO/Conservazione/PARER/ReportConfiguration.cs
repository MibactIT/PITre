using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace DocsPaVO.Conservazione.PARER
{
    public class ReportConfiguration
    {
        private String _idAmm;
        private String _subject;
        private String _body;
        private String[] _recipients;
        private String[] _fixed_recipients;
        private Mailbox _mbox;

        public String idAmm
        {
            get
            {
                return _idAmm;
            }
            set
            {
                _idAmm = value;
            }
        }

        public String Subject
        {
            get
            {
                return _subject;
            }
            set
            {
                _subject = value;
            }
        }

        public String Body
        {
            get
            {
                return _body;
            }
            set
            {
                _body = value;
            }
        }

        public String[] Recipients
        {
            get
            {
                return _recipients;
            }
            set
            {
                _recipients = value;
            }
        }


        public String[] FixedRecipients
        {
            get
            {
                return _fixed_recipients;
            }
            set
            {
                _fixed_recipients = value;
            }
        }

        public Mailbox MailBoxConfiguration
        {
            get
            {
                return _mbox;
            }
            set
            {
                _mbox = value;
            }
        }
    }
}
