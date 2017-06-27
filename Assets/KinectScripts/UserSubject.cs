using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.KinectScripts
{
    interface UserSubject
    {
         void addObserver(UserObserver newObserver);
         void notifyObserver();
    }
}
