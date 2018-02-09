using System;
using System.Collections.Generic;
using UnityEngine;

namespace AssemblyCSharp {

    //game performance recording session, with a list of the recorded metrics
    [Serializable]
    public class Session {

        public string _id;
        //the version of the corresponding build
        public string build;
        //the name of the game
        public string game;
        //the scene choosen for recording
        public string scene;
        //the start date of the session
        public DateTime startDate;
        //the duration of the session, in milliseconds
        public uint duration;
        //the list of the recorded metrics
        public List<Metric> metrics;

        public Session() {
            //build = Application.version;
            build = "buildVersion";
            //game = Application.productName;
            game = "gameName";
            scene = "sceneName";
            startDate = System.DateTime.Now;
            duration = 0;
            string formatedStartDate = 
                startDate.Year.ToString("D4") + startDate.Month.ToString("D2") + startDate.Day.ToString("D2") +
                startDate.Hour.ToString("D2") + startDate.Minute.ToString("D2") + startDate.Second.ToString("D2");
            //generates an id with the session details
            _id = game + "_" + scene + "_" + formatedStartDate;
            metrics = new List<Metric>();
        }

        public override string ToString() {
            string str = _id + "\n" +
                         build + " - " + game + " - " + scene + "\n" +
                         startDate + " - " + duration + "\n" +
                         "Metrics:\n";
            foreach(Metric metric in metrics) {
                str += metric.ToString() + "\n";
            }
            return str;
        }

        /*CRUD*/

        public bool createMetric(Metric metric) {
            if(!metrics.Contains(metric)) {
                metrics.Add(metric);
                return true;
            }
            return false;
        }

        public bool deleteMetric(Metric metric) {
            if(metrics.Contains(metric))
                return metrics.Remove(metric);
            return false;
        }
    }
}
