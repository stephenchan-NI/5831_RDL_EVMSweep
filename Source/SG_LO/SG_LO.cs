using NationalInstruments.DataInfrastructure;
using NationalInstruments.ModularInstruments.NIRfsg;
using NationalInstruments.ModularInstruments.NIRfsgPlayback;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Text.RegularExpressions;

namespace NationalInstruments.ReferenceDesignLibraries
{
    public class SG_LO
    {
        NIRfsg _rfsgSession;
        #region Program Functions
        public SG_LO(string rfsgName)
        {
            // Initialize the NIRfsg session
            _rfsgSession = new NIRfsg(rfsgName, true, false);
            _rfsgSession.FrequencyReference.ExportedOutputTerminal = RfsgFrequencyReferenceExportedOutputTerminal.ReferenceOut;
        }

        public void StartGeneration(double freq, out double pow)
        {
            double powerOut = _rfsgSession.RF.PowerLevel;
            // Configure the instrument 
            _rfsgSession.RF.Frequency = freq;
            if (powerOut  > 10 )
            {
                pow = 10;
            }
            else if (powerOut < 0)
            {
                pow = 0;
            }
            else
            {
                pow = powerOut;
            }
            // Initiate Generation 
            _rfsgSession.Initiate();
        }

        public void StopGeneration()
        {
            if (_rfsgSession != null)
            {
                // Disable the output.  This sets the noise floor as low as possible.
                _rfsgSession.RF.OutputEnabled = false;

                // Close the RFSG NIRfsg session
                _rfsgSession.Close();
                }
            _rfsgSession = null;
        }

        public void UpdateGeneration(double freq, out double pow)
        {
            // Abort generation 
            _rfsgSession.Abort();

            // Configure the instrument 
            double powerOut = _rfsgSession.RF.PowerLevel;
            _rfsgSession.RF.Frequency = freq;
            if (powerOut > 10)
            {
                pow = 10;
            }
            else if (powerOut < 0)
            {
                pow = 0;
            }
            else
            {
                pow = powerOut;
            }

            // Initiate Generation 
            _rfsgSession.Initiate();
        }
    }
}
#endregion