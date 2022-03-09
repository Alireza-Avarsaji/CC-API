using System;
using System.Collections.Generic;
using System.Text;

namespace CC_Proj1_Alireza.Dtos
{
    public class NaturalLanguageDto
    {
        public string language { get; set; }
        public List<Keyword> keywords { get; set; }
    }


    public class Sentiment
    {
        public double score { get; set; }
        public string label { get; set; }
    }

    public class Emotion
    {
        public double sadness { get; set; }
        public double joy { get; set; }
        public double fear { get; set; }
        public double disgust { get; set; }
        public double anger { get; set; }
    }

    public class Keyword
    {
        public string text { get; set; }
        public Sentiment sentiment { get; set; }
        public double relevance { get; set; }
        public Emotion emotion { get; set; }
        public int count { get; set; }
    }




}