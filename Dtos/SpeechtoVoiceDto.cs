using System;
using System.Collections.Generic;
using System.Text;

namespace CC_Proj1_Alireza.Dtos
{

    public class Alternative
    {
        public string transcript { get; set; }
        public double confidence { get; set; }
    }

    public class TextRes
    {
        public bool final { get; set; }
        public List<Alternative> alternatives { get; set; }
    }

    public class SpeechtoVoiceDto
    {
        public int result_index { get; set; }
        public List<TextRes> results { get; set; }
    }






}