using System;
using System.Collections.Generic;
using System.Text;

namespace CC_Proj1_Alireza.Dtos
{
    public class Translation
    {
        public string translation { get; set; }
    }

    public class TranslateDto
    {
        public List<Translation> translations { get; set; }
        public int word_count { get; set; }
        public int character_count { get; set; }
    }
}