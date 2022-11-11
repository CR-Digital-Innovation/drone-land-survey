using System;
using log4net;
using log4net.Config;

namespace Cr.Igrs.CloudSync.Log
{
    public class Logger
    {

        private readonly ILog logger =
        LogManager.GetLogger("Cr.LogNet");

        public Logger()
        {

        }
        static Logger()
        {
            XmlConfigurator.Configure();
        }



        public void LogException(Exception ex, int errorCode)
        {
            try
            {
                log4net.ThreadContext.Properties["EventID"] = errorCode;
                logger.Error(String.Format("Code : {0}, message : {1}", errorCode, ex.Message), ex);
                log4net.ThreadContext.Properties["EventID"] = 0;
            }
            catch (Exception)
            {

            }

        }

        public void LogError(string v1, int v2, object cAT_ERR_LOG)
        {
            throw new NotImplementedException();
        }

        public void LogException(Exception ex)
        {
            try
            {
                logger.Error(ex.Message, ex);
                //LogManager.LogError(ex, 1000, IIFConstants.CAT_ERR_LOG);
            }
            catch (Exception)
            {

            }
        }

        public void LogError(string message, int errorCode)
        {
            try
            {
                log4net.ThreadContext.Properties["EventID"] = errorCode;
                logger.Error(String.Format("Code : {0}, message : {1}", errorCode, message));
                log4net.ThreadContext.Properties["EventID"] = 0;
            }
            catch (Exception)
            {

            }
        }

        #region LogError
        /// <summary>
        /// Log the error encountered
        /// </summary>
        public void LogError(String message, params string[] list)
        {
            try
            {
                message = string.Format(message, list);
            }
            catch (Exception)
            {

            }
            LogError(message);
        }
        #endregion

        public void LogTrace(String message, params string[] list)
        {
            try
            {
                message = string.Format(message, list);
            }
            catch (Exception)
            {
            }
            LogTrace(message);
        }

        public void LogError(string message)
        {
            try
            {

                logger.Error(message);
            }
            catch (Exception)
            {

            }
        }

        public void LogTrace(string message)
        {
            try
            {
                //LogManager.LogEvent(message, EventType.Information, category);

                logger.Info(message);
            }
            catch (Exception)
            {

            }
        }

        public void LogInformation(string message)
        {
            try
            {
                //LogManager.LogEvent(message, EventType.Information, IIFConstants.CAT_INFO_LOG);

                logger.Info(message);
            }
            catch (Exception)
            {

            }
        }

        public void LogInformation(string message, EventType eventType)
        {
            try
            {
                if (eventType == EventType.Debug)
                {
                    logger.Debug(message);
                }
                else
                {
                    logger.Info(message);
                }
            }
            catch (Exception)
            {

            }
        }
    }

    public enum EventType
    {
        Information,
        Debug
    }

}