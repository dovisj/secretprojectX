using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Timers
{
   public static string Countdown(float timer)
   {
      int minutes = Mathf.FloorToInt(timer / 60F);
      int seconds = Mathf.FloorToInt(timer - minutes * 60);
      return string.Format("{0:0}:{1:00}", minutes, seconds);
   }
}
