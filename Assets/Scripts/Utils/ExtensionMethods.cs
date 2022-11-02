using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Utils
{
    public static class ExtensionMethods
    {
        public static int CompareToWithApproximation(this float value, float compareValue, float approximation)
        {
            if (value + approximation < compareValue)
            {
                return -1;
            }

            if (value - approximation > compareValue)
            {
                return 1;
            }

            if (value - approximation <= compareValue && compareValue <= value + approximation)
            {
                return 0;
            }

            if (float.IsNaN(compareValue))
            {
                if (!float.IsNaN(compareValue))
                {
                    return -1;
                }

                return 0;
            }

            return 1;
        }

        public static int GetSidewaysOrientation(this Direction direction)
        {
            if (direction == Direction.RIGHT)
                return 1;
            else if (direction == Direction.LEFT)
                return -1;
            else
                return 0;
        }
    }
}