using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuildYourOwnMessenger.Messages
{
    public record OnlineStatusChangedMessage(bool IsOnline)
    {
        
    }
}
