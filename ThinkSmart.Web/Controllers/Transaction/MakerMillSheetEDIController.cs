using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Utility;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class MakerMillSheetEDIController : Controller
    {
        private readonly IWebHostEnvironment _environment;

        public MakerMillSheetEDIController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public IActionResult MakerMillSheetEDIBatch()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }

        [HttpPost]
        public ActionResult ReadTextFile(IFormFile file)
        {


            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var list = new List<MakerMillSheetEDIDetails>();
            var allLines = new List<string>();
            string errorMessage = string.Empty;

            using (var reader = new StreamReader(file.OpenReadStream(), Encoding.GetEncoding(932)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (!string.IsNullOrWhiteSpace(line))
                        allLines.Add(line);
                }
            }

            // Every 3 lines = 1 record
            for (int i = 0; i < allLines.Count; i += 3)
            {
                if (i + 2 >= allLines.Count)
                    break;

                var line1 = allLines[i];
                var line2 = allLines[i + 1];
                var line3 = allLines[i + 2];

                var obj = new MakerMillSheetEDIDetails();

                //Values taken hard coded from  windows table MST_MAKER_MILLSHEET_EDI_FORMAT
                // Hardcoded field extraction

                //obj.FILENAME = file.FileName;

                // MATL_NAME_MAKER → Line 1, Start 50, End 69
                obj.MATL_NAME_MAKER = GetValue<string>(line1, 50, 69);

                // CHARGE → Line 1, Start 0, End 10
                obj.CHARGE = GetValue<string>(line1, 9, 16);

                // DIM_D → Line 1, Start 20, End 25
                obj.DIM_D = GetValue<decimal>(line1, 89, 100);

                // GRAIN_SIZE_STD → Line 1, Start 344, End 346
                obj.GRAIN_SIZE_STD = GetValue<decimal>(line1, 344, 346);

                // HARDNESS_STD → Line 2, Start 301, End 303
                obj.HARDNESS_STD = GetValue<string>(line2, 301, 303);

                // HARDNESS_ACTUAL → Line 2, Start 20, End 25
                obj.HARDNESS_ACTUAL = GetValue<string>(line2, 304, 307);

                // CCNAME1 → Line 1, Start 104, End 105
                obj.CCNAME1 = GetValue<string>(line1, 104, 105);

                // CCRANGE1 → Line 2, Start 253, End 253
                obj.CCRANGE1 = GetValue<string>(line2, 253, 253);

                // CCACTUAL1 → Line 1, Start 106, End 109
                obj.CCACTUAL1 = GetValue<string>(line1, 106, 109);

                // CCNAME2 → Line 1, Start 110, End 111
                obj.CCNAME2 = GetValue<string>(line1, 110, 111);

                // CCRANGE2 → Line 2, Start 254, End 254
                obj.CCRANGE2 = GetValue<string>(line2, 254, 254);

                // CCACTUAL2 → Line 1, Start 112, End 115
                obj.CCACTUAL2 = GetValue<string>(line1, 112, 115);

                // CCNAME3 → Line 1, Start 116, End 117
                obj.CCNAME3 = GetValue<string>(line1, 116, 117);

                // CCRANGE3 → Line 2, Start 255, End 255
                obj.CCRANGE3 = GetValue<string>(line2, 255, 255);

                // CCACTUAL3 → Line 1, Start 118, End 121
                obj.CCACTUAL3 = GetValue<string>(line1, 118, 121);

                // CCNAME4 → Line 1, Start 122, End 123
                obj.CCNAME4 = GetValue<string>(line1, 122, 123);

                // CCRANGE4 → Line 2, Start 256, End 256
                obj.CCRANGE4 = GetValue<string>(line2, 256, 256);

                // CCACTUAL4 → Line 1, Start 124, End 127
                obj.CCACTUAL4 = GetValue<string>(line1, 124, 127);

                // CCNAME5 → Line 1, Start 128, End 129
                obj.CCNAME5 = GetValue<string>(line1, 128, 129);

                // CCRANGE5 → Line 2, Start 257, End 257
                obj.CCRANGE5 = GetValue<string>(line2, 257, 257);

                // CCACTUAL5 → Line 1, Start 130, End 133
                obj.CCACTUAL5 = GetValue<string>(line1, 130, 133);

                // CCNAME6 → Line 1, Start 134, End 135
                obj.CCNAME6 = GetValue<string>(line1, 134, 135);

                // CCRANGE6 → Line 2, Start 258, End 258
                obj.CCRANGE6 = GetValue<string>(line2, 258, 258);

                // CCACTUAL6 → Line 1, Start 136, End 139
                obj.CCACTUAL6 = GetValue<string>(line1, 136, 139);

                // CCNAME7 → Line 1, Start 140, End 141
                obj.CCNAME7 = GetValue<string>(line1, 140, 141);

                // CCRANGE7 → Line 2, Start 259, End 259
                obj.CCRANGE7 = GetValue<string>(line2, 259, 259);

                // CCACTUAL7 → Line 1, Start 142, End 145
                obj.CCACTUAL7 = GetValue<string>(line1, 142, 145);

                // CCNAME8 → Line 1, Start 146, End 147
                obj.CCNAME8 = GetValue<string>(line1, 146, 147);

                // CCRANGE8 → Line 2, Start 260, End 260
                obj.CCRANGE8 = GetValue<string>(line2, 260, 260);

                // CCACTUAL8 → Line 1, Start 148, End 151
                obj.CCACTUAL8 = GetValue<string>(line1, 148, 151);

                // CCNAME9 → Line 1, Start 152, End 153
                obj.CCNAME9 = GetValue<string>(line1, 152, 153);

                // CCRANGE8 → Line 2, Start 261, End 261
                obj.CCRANGE9 = GetValue<string>(line2, 261, 261);

                // CCACTUAL9 → Line 1, Start 154, End 157
                obj.CCACTUAL9 = GetValue<string>(line1, 154, 157);

                // CCNAME10 → Line 1, Start 158, End 159
                obj.CCNAME10 = GetValue<string>(line1, 158, 159);

                // CCRANGE10 → Line 2, Start 262, End 262
                obj.CCRANGE10 = GetValue<string>(line2, 262, 262);

                // CCACTUAL10 → Line 1, Start 160, End 163
                obj.CCACTUAL10 = GetValue<string>(line1, 160, 163);

                // CCNAME11 → Line 1, Start 164, End 165
                obj.CCNAME11 = GetValue<string>(line1, 164, 165);

                // CCRANGE11 → Line 2, Start 263, End 263
                obj.CCRANGE11 = GetValue<string>(line2, 263, 263);

                // CCACTUAL11 → Line 1, Start 166, End 169
                obj.CCACTUAL11 = GetValue<string>(line1, 166, 169);

                // CCNAME12 → Line 1, Start 170, End 171
                obj.CCNAME12 = GetValue<string>(line1, 170, 171);

                // CCRANGE12 → Line 2, Start 264, End 264
                obj.CCRANGE12 = GetValue<string>(line2, 264, 264);

                // CCACTUAL12 → Line 1, Start 172, End 175
                obj.CCACTUAL12 = GetValue<string>(line1, 172, 175);

                // CCNAME13 → Line 1, Start 176, End 177
                obj.CCNAME13 = GetValue<string>(line1, 176, 177);

                // CCRANGE13 → Line 2, Start 265, End 265
                obj.CCRANGE13 = GetValue<string>(line2, 265, 265);

                // CCACTUAL13 → Line 1, Start 178, End 181
                obj.CCACTUAL13 = GetValue<string>(line1, 178, 181);

                // CCNAME14 → Line 1, Start 182, End 183
                obj.CCNAME14 = GetValue<string>(line1, 182, 183);

                // CCRANGE14 → Line 2, Start 266, End 266
                obj.CCRANGE14 = GetValue<string>(line2, 266, 266);

                // CCACTUAL14 → Line 1, Start 184, End 187
                obj.CCACTUAL14 = GetValue<string>(line1, 184, 187);

                // CCNAME15 → Line 1, Start 188, End 189
                obj.CCNAME15 = GetValue<string>(line1, 188, 189);

                // CCRANGE15 → Line 2, Start 267, End 267
                obj.CCRANGE15 = GetValue<string>(line2, 267, 267);

                // CCACTUAL15 → Line 1, Start 190, End 193
                obj.CCACTUAL15 = GetValue<string>(line1, 190, 193);



                // HARD_ACTUAL1 → Line 2, Start 1, End 2
                obj.HARD_ACTUAL1 = GetValue<decimal>(line2, 1, 2);

                // HARD_ACTUAL2 → Line 2, Start 3, End 4
                obj.HARD_ACTUAL2 = GetValue<decimal>(line2, 3, 4);

                // HARD_ACTUAL3 → Line 2, Start 5, End 6
                obj.HARD_ACTUAL3 = GetValue<decimal>(line2, 5, 6);

                // HARD_ACTUAL4 → Line 2, Start 7, End 8
                obj.HARD_ACTUAL4 = GetValue<decimal>(line2, 7, 8);

                // HARD_ACTUAL5 → Line 2, Start 9, End 10
                obj.HARD_ACTUAL5 = GetValue<decimal>(line2, 9, 10);

                // HARD_ACTUAL6 → Line 2, Start 11, End 12
                obj.HARD_ACTUAL6 = GetValue<decimal>(line2, 11, 12);

                // HARD_ACTUAL7 → Line 2, Start 13, End 14
                obj.HARD_ACTUAL7 = GetValue<decimal>(line2, 13, 14);

                // HARD_ACTUAL8 → Line 2, Start 15, End 16
                obj.HARD_ACTUAL8 = GetValue<decimal>(line2, 15, 16);

                // HARD_ACTUAL9 → Line 2, Start 17, End 18
                obj.HARD_ACTUAL9 = GetValue<decimal>(line2, 17, 18);

                // HARD_ACTUAL10 → Line 2, Start 19, End 20
                obj.HARD_ACTUAL10 = GetValue<decimal>(line2, 19, 20);

                // HARD_ACTUAL11 → Line 2, Start 21, End 22
                obj.HARD_ACTUAL11 = GetValue<decimal>(line2, 21, 22);

                // HARD_ACTUAL12 → Line 2, Start 23, End 24
                obj.HARD_ACTUAL12 = GetValue<decimal>(line2, 23, 24);

                // HARD_ACTUAL13 → Line 2, Start 25, End 26
                obj.HARD_ACTUAL13 = GetValue<decimal>(line2, 25, 26);

                // HARD_ACTUAL14 → Line 2, Start 27, End 28
                obj.HARD_ACTUAL14 = GetValue<decimal>(line2, 27, 28);

                // HARD_ACTUAL15 → Line 2, Start 29, End 30
                obj.HARD_ACTUAL15 = GetValue<decimal>(line2, 29, 30);

                // LENGTH → Line , Start , End 
                // obj.LENGTH = GetValue<decimal>(line, , );



                list.Add(obj);
            }

            //Values taken hard coded from  windows table MST_MAKER_MILLSHEET_EDI_FORMAT
            var ccNameMap = new Dictionary<string, string>()
             {
                 { "1", "C" },
                 { "2", "Si" },
                 { "3", "Mn" },
                 { "4", "P" },
                 { "5", "S" },
                 { "6", "Cu" },
                 { "7", "Ni" },
                 { "8", "Cr" },
                 { "9", "Mo" },
                 { "10", "Pb" },
                 { "11", "W" },
                 { "12", "V" },
                 { "13", "Co" },
                 { "14", "As" },
                 { "15", "Sn" },
                 { "16", "Al " },

                 { "0A", "T-Al" },

                 { "17", "S-AL" },
                 { "18", "I-AL" },
                 { "19", "TI" },
                 { "20", "Nb" },
                 { "21", "Cb" },
                 { "22", "Te" },
                 { "23", "B" },

                 { "0B", "T-B" },

                 { "24", "S-B" },
                 { "25", "I-B" },
                 { "26", "Ca" },
                 { "27", "O" },
                 { "28", "N" },

                 { "0C", "T-N" },

                 { "29", "S-N" },
                 { "30", "I-N" },
                 { "31", "SB" },
                 { "32", "Se" },
                 { "33", "Mg" },
                 { "34", "Fe" },
                 { "35", "Ta" },
                 { "36", "Zr" },
                 { "37", "Be" },
                 { "38", "Zn" },
                 { "39", "Ce" },
                 { "40", "Ag" },
                 { "41", "Bi" },
                 { "42", "H" },
                 { "43", "Y" },
                 { "44", "Sm" },
                 { "45", "Hg" },
                 { "46", "Hf" },
                 { "47", "Cd" },
                 { "48", "Re" },
                 { "49", "La " },

                 { "0D", "Ge" },
                 { "0F", "Nd" },
                 { "0G", "Pr" },

                 { "50", "Pd" },
                 //{ "51", " " },
                 { "52", "Cr+Mo" },
                 { "53", "PT" },
                 { "54", "In" },
                 { "55", "Ni+Cr" }
             };

            foreach (var obj in list)
            {
                var properties = obj.GetType().GetProperties()
                                    .Where(p => p.Name.StartsWith("CCNAME"));

                foreach (var prop in properties)
                {
                    var value = prop.GetValue(obj)?.ToString();

                    if (!string.IsNullOrEmpty(value) && ccNameMap.TryGetValue(value, out string actualName))
                    {
                        prop.SetValue(obj, actualName);
                    }
                }
            }

            return Ok(new
            {
                isSuccess = true,
                data = list
            });
        }

        private T GetValue<T>(string line, int start, int end)
        {
            string errorMessage = string.Empty;
            try
            {
                int length = (end - start) + 1;

                if (line.Length < start - 1 + length)
                {
                    errorMessage = "Not Found";
                    return default;
                }

                string value = line.Substring(start - 1, length).Trim();

                value = value.TrimStart('0').TrimStart('.').TrimStart('0');

                if (string.IsNullOrEmpty(value))
                {
                    errorMessage = "Not Found";
                    return default;
                }

                if (typeof(T) == typeof(string))
                    return (T)(object)value;

                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                {
                    errorMessage = "Not Found";
                    return default;
                }
            }
        }
    }
}
