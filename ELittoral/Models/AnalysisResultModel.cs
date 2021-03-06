﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELittoral.Models
{
    public class AnalysisResultModel : ImageModel
    {
        public int Id { get; set; }

        public AnalysisModel Analysis { get; set; }

        public string Result { get; set; }

        public ReconResourceModel MinuendRessource { get; set; }

        public ReconResourceModel SubtrahendRessource { get; set; }

    }
}
