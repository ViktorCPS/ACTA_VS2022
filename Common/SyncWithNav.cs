using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Runtime.Serialization;
using DataAccess;
using TransferObjects;
using Util;


namespace Common
{
    [Serializable()]
    /// <summary>
    /// Klasa za pozivanje stornih procedura iz baze
    /// </summary>
    public class SyncWithNav : ISerializable
    {
        DAOFactory daoFactory = null;
        SyncWithNavDAO edao = null;

        DebugLog log;
        int brPokusaja;
        int rez;



        public SyncWithNav()
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "LogSinhronizacije.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getSyncWithNavDAO(null);
            rez = 0;
            brPokusaja = 0;
        }

        public SyncWithNav(bool createDAO)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "LogSinhronizacije.txt";
            log = new DebugLog(logFilePath);

            if (createDAO)
            {
                daoFactory = DAOFactory.getDAOFactory();
                edao = daoFactory.getSyncWithNavDAO(null);
            }
            rez = 0;
            brPokusaja = 0;
        }

        public SyncWithNav(Object dbConnection)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "LogSinhronizacije.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getSyncWithNavDAO(dbConnection);
            rez = 0;
            brPokusaja = 0;
        }
        public SyncWithNav(int r)
        {
            string logFilePath = Constants.logFilePath + NotificationController.GetApplicationName() + "LogSinhronizacije.txt";
            log = new DebugLog(logFilePath);

            daoFactory = DAOFactory.getDAOFactory();
            edao = daoFactory.getSyncWithNavDAO(null);

            rez = r;
            brPokusaja = 0;
        }


        public void GetObjectData(SerializationInfo info, StreamingContext ctxt)
        {
            //Get the values from info and assign them to the appropriate properties
            this.rez = (int)info.GetValue("rez", typeof(int));
        }


        public List<int> SinhronizujPodatke()
        {
            List<int> brPodataka = new List<int>();
            try
            {
                log.writeLog("+ INSERTING NEW DATA - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Inserting of working units - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiNoveRJ"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiVezuRJsaOp"));
                log.writeLog("\t- Inserting of working units - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Inserting of organizational units - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiNoveOJ"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiVezuNOJsaOp"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiVezuNRJsaOJ"));
                log.writeLog("\t- Inserting of organizational units - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Inserting of employees - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiSrednjeImeDupZap"));//4
                brPodataka.Add(edao.PozoviStornuProceduru("AzurirajNaloge")); //1
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiNoveZap"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiZapULok"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiDodPodZap"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiDodPodZaNepZap"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiDefRadnuSemu"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiBrojaceZap"));
                log.writeLog("\t- Inserting of employees - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("- INSERTING NEW DATA - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("+ UPDATING DATA - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Updating of working units - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiVezuRJsaOpZaPrNaRJ"));
                brPodataka.Add(edao.PozoviStornuProceduru("AzurirajRJ"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiVezuRJsaOp"));
                log.writeLog("\t- Updating of working units - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Updating of organizational units - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiVezuOJsaOpZaPrNaOJ"));
                brPodataka.Add(edao.PozoviStornuProceduru("AzurirajOJ"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiVezuNOJsaOp"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiVezuNRJsaOJ"));
                log.writeLog("\t- Updating of organizational units - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Updating of employees - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("AzurirajNaloge"));
                brPodataka.Add(edao.PozoviStornuProceduru("AzurirajZap"));
                brPodataka.Add(edao.PozoviStornuProceduru("AzurirajDodPodZap"));
                brPodataka.Add(edao.PozoviStornuProceduru("AzurirajBrojaceZap"));
                log.writeLog("\t- Updating of employees - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("- UPDATING DATA - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("+ DELETING DATA - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Deleting of working units - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiRJ"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiVezuRJsaOp"));
                log.writeLog("\t- Deleting of working units - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Deleting of organizational units - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiOJ"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiVezuOJsaOp"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiVezuRJsaOJ"));
                log.writeLog("\t- Deleting of organizational units - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("\t+ Deleting of employees - STARTED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
                brPodataka.Add(edao.PozoviStornuProceduru("UnesiDefRadnuSemuZaOtpustene"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiZap"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiTagZaOtpZap"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiNalogZaOtpZap"));
                brPodataka.Add(edao.PozoviStornuProceduru("ObrisiSmenePosOtkZap"));
                log.writeLog("\t- Deleting of employees - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));

                log.writeLog("- DELETING DATA - FINISHED! " + DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            }
            catch (Exception ex)
            {
                log.writeLog(DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss") + " SYNCHRONIZATION - FAILED " + ex.Message);
                if (brPokusaja++ < 3)
                {
                    //Dodati da se petlja uspava na 10-ak minuta
                    brPodataka.Add(111111);//da napravi razliku ukoliko 
                    brPodataka = SinhronizujPodatke();
                }
            }
            return brPodataka;

        }
    }
}
