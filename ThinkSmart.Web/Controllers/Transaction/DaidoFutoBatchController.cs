using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TSWebApplication.Domain.Model;
using TSWebApplication.Domain.Resources;
using TSWebApplication.Domain.Utility;
using TSWebApplication.Infrastructure.IRepositories;
using TSWebApplication.Infrastructure.Repositories;
using static Google.Cloud.Vision.V1.ProductSearchResults.Types;

namespace TSWebApplication.Web.Controllers.Transaction
{
    public class DaidoFutoBatchController : Controller
    {
        private readonly IAllCommonRepository _CommonRepository;
        private readonly LocalizationServices _localization;
        private readonly IDaidoFutoBatchRepository _daidoFutoBatchRepository;
        string m_textDataErrorMessage = string.Empty;
        public DaidoFutoBatchController(IAllCommonRepository commonRepository, LocalizationServices localization, IDaidoFutoBatchRepository daidoFutoBatchRepository)
        {
            _localization = localization;
            _CommonRepository = commonRepository;
            _daidoFutoBatchRepository = daidoFutoBatchRepository;

        }
        public IActionResult DaidoFutoImportBatch()
        {
            ViewBag.CultureName = UIHelper.GetCultureName();
            return View();
        }       
        public IActionResult DaidoFutouBatchReport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ReadTextFile(IFormFile file,int futouCd)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            int rowIndex = 0;
            var list = new List<DaidoFutouNyukoDetails>();

            using (var reader = new StreamReader(
                       file.OpenReadStream(),
                       Encoding.GetEncoding(932)))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                        continue;


                    if (line.Length != 258)
                        continue;

                    var obj = new DaidoFutouNyukoDetails();

                    string getValue;
                  
                    //string textDataErrorMessage = "";

                    bool isKouban = false;
                    string kouban = "";
                    string materialNameMak = "";



                    // ===================================================
                    // 1. TYPE_OF_DATA
                    // 0 - 1
                    // ===================================================

                    //getValue = GetValueFromReadLineText(0, 1, line);

                    //obj.TYPE_OF_DATA = getValue;
                    getValue = GetValueFromReadLineText(0, 1, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;                       
                    }

                    obj.TYPE_OF_DATA = getValue;


                    // ===================================================
                    // 2. FUTO_NYUKO_DATE
                    // 1 - 7
                    // ===================================================

                    getValue = GetValueFromReadLineText(1, 7, line);

                    if (TryParseYYMMDD(getValue, out DateTime parsedDate))
                    {
                       
                    }
                    else
                    {
                        obj.ERROR = "Format Error";
                    }
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage)) 
                    {
                        obj.ERROR = m_textDataErrorMessage; 
                    }
                    obj.FUTO_NYUKO_DATE = parsedDate;

                    // ===================================================
                    // 3. CONTRACT_NUMBER
                    // 7 - 19
                    // ===================================================

                    getValue = GetValueFromReadLineText(7, 19, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }

                    obj.CONTRACT_NUMBER = getValue;


                    // ===================================================
                    // 4. CONTRACT_GYOUBAN
                    // 19 - 21
                    // ===================================================

                    getValue = GetValueFromReadLineText(19, 21, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                   
                    obj.CONTRACT_GYOUBAN = getValue;


                    // ===================================================
                    // 5. FUTO_NYUKO_LOCATION
                    // 21 - 27
                    // ===================================================

                    getValue = GetValueFromReadLineText(21, 27, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }

                    obj.FUTO_NYUKO_LOCATION = getValue;


                    // ===================================================
                    // 6. RECEIVER
                    // 27 - 33
                    // ===================================================

                    getValue = GetValueFromReadLineText(27, 33, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }

                    obj.RECEIVER = getValue;


                    // ===================================================
                    // 7. MUKESAKI
                    // 33 - 39
                    // ===================================================

                    getValue = GetValueFromReadLineText(33, 39, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }


                    obj.MUKESAKI = getValue;


                    // ===================================================
                    // 8. MAKER_CODE
                    // 39 - 43
                    // ===================================================

                    getValue = GetValueFromReadLineText(39, 43, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }

                    obj.MAKER_CODE = getValue;


                    // ===================================================
                    // 9. CONTRACTOR
                    // 43 - 49
                    // ===================================================

                    getValue = GetValueFromReadLineText(43, 49, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }

                    obj.CONTRACTOR = getValue;


                    // ===================================================
                    // 10. PROCESSING_PERSON
                    // 49 - 55
                    // ===================================================

                    getValue = GetValueFromReadLineText(49, 55, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }

                    obj.PROCESSING_PERSON = getValue;


                    // ===================================================
                    // 11. ORDER_KOUBAN
                    // 55 - 62
                    // ===================================================

                    getValue = GetValueFromReadLineText(55, 62, line);


                    bool koubanExists = _daidoFutoBatchRepository.IsKoubanExists(getValue);

                    if (!koubanExists)
                        {
                            isKouban = false;

                        m_textDataErrorMessage = _localization.GetLocalizedHtmlString("valMsg_KoubanNotFoundForDaido");
                       
                    }
                       
                            bool isDeleted = _daidoFutoBatchRepository.IsKoubanDeleted(getValue);

                            if (isDeleted)
                            {
                                isKouban = false;

                        m_textDataErrorMessage = _localization.GetLocalizedHtmlString("valMsg_KoubanCompletedCanNotImport");
                                   
                            }
                       
                    

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                        obj.ORDER_KOUBAN = getValue;
                    }
                    else
                    {
                        isKouban = true;
                        kouban = getValue;

                        obj.ORDER_KOUBAN = getValue;
                    }

                    // ===================================================
                    // 12. STEEL_GRADE
                    // 62 - 82
                    // ===================================================

                    getValue = GetValueFromReadLineText(62, 82, line);

                    if (!string.IsNullOrEmpty(kouban))
                    {
                        bool materialExists = _daidoFutoBatchRepository.IsMaterialNameMakerExists(kouban, getValue);

                        if (!materialExists)
                        {
                            m_textDataErrorMessage = _localization.GetLocalizedHtmlString("valMsg_KoubanMatlNameDimMisMatchCanNotImport");
                                
                        }
                    }

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                        obj.STEEL_GRADE = getValue;
                    }
                    else
                    {
                        materialNameMak = getValue;
                        obj.STEEL_GRADE = getValue;
                    }


                    // ===================================================
                    // 13. DIM_IMAGE
                    // 82 - 102
                    // ===================================================

                    getValue = GetValueFromReadLineText(82, 102, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.DIM_IMAGE = getValue;


                    // ===================================================
                    // 14. DELIVERY_CONDITION
                    // 102 - 121
                    // ===================================================

                    getValue = GetValueFromReadLineText(102, 121, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.DELIVERY_CONDITION = getValue;


                    // ===================================================
                    // 15. PROD_LENGTH
                    // 121 - 141
                    // ===================================================

                    getValue = GetValueFromReadLineText(121, 141, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.WINDING_DIRECTION = getValue;


                    // ===================================================
                    // 16. PROD_SPEC_KOUBAN
                    // 141 - 151
                    // ===================================================

                    getValue = GetValueFromReadLineText(141, 151, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.PROD_SPEC_KOUBAN = getValue;


                    // ===================================================
                    // 17. CHARGE
                    // 151 - 158
                    // ===================================================

                    getValue = GetValueFromReadLineText(151, 158, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.CHARGE = getValue;


                    // ===================================================
                    // 18. THABA_NO
                    // 158 - 161
                    // ===================================================

                    getValue = GetValueFromReadLineText(158, 161, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.THABA_NO = getValue;


                    // ===================================================
                    // 19. NO_OF_UNITS
                    // 161 - 168
                    // ===================================================

                    getValue = GetValueFromReadLineText(161, 168, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.NO_OF_UNITS = getValue;


                    // ===================================================
                    // 20. QUANTITY_IN_KG
                    // 168 - 176
                    // ===================================================

                    getValue = GetValueFromReadLineText(168, 176, line);

                    if (!string.IsNullOrEmpty(getValue))
                    {
                        string checkMark = getValue.Substring(0, 1);

                        if (checkMark == "-")
                        {
                            m_textDataErrorMessage = _localization.GetLocalizedHtmlString("valMsg_KoubanWeightNegativeCanNotImport");
                                
                        }
                    }

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    else 
                    {
                        obj.QUANTITY_IN_KG = getValue;
                    }

                    // ===================================================
                    // 21. YARD_KBN
                    // 176 - 177
                    // ===================================================

                    getValue = GetValueFromReadLineText(176, 177, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.YARD_KBN = getValue;

                    // ===================================================
                    // 22. NYUKA_KBN
                    // 1767- 178
                    // ===================================================
                    getValue = GetValueFromReadLineText(177, 178, line);

                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.NYUKA_KBN = getValue;

                    // ===================================================
                    // 23. SHUKKA_KBN
                    // 176 - 177
                    // ===================================================
                    getValue = GetValueFromReadLineText(178, 179, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.SHUKKA_KBN = getValue;

                    // ===================================================
                    // 24. USER_MANAGEMENT_KEY
                    // 179 - 189
                    // ===================================================
                    getValue = GetValueFromReadLineText(179, 189, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.USER_MANAGEMENT_KEY = getValue;

                    // ===================================================
                    // 25. DIMD_DECOMPOSITION_SIZE
                    // 189 - 208
                    // ===================================================
                    // 25. DIMD_DECOMPOSITION_SIZE
                    getValue = GetValueFromReadLineText(189, 208, line);

                    // Check Dim-D
                    if (!string.IsNullOrEmpty(kouban))
                    {
                        bool koubanMaterialNameMakDimDExists = _daidoFutoBatchRepository.IsKoubanMaterialNameMakDimDExists(kouban, materialNameMak, getValue);
                        if (!koubanMaterialNameMakDimDExists)
                        {
                            m_textDataErrorMessage = _localization.GetLocalizedHtmlString("valMsg_KoubanMatlNameDimMisMatchCanNotImport");
                                
                        }
                    }

                    // Add error message
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                        obj.DIMD_DECOMPOSITION_SIZE = getValue;
                    }
                    else 
                    {                       
                        obj.DIMD_DECOMPOSITION_SIZE = getValue;

                    }

                    // ===================================================
                    // 26. SHAPE_INFO
                    // 208 - 211
                    // ===================================================
                    getValue = GetValueFromReadLineText(208, 211, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.SHAPE_INFO = getValue;

                    // ===================================================
                    // 27. ORDER_PERSON_CODE
                    // 211 - 214
                    // ===================================================
                    getValue = GetValueFromReadLineText(211, 214, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.ORDER_PERSON_CODE = getValue;

                    // ===================================================
                    // 28. RESERVE
                    // 214 - 216
                    // ===================================================
                    getValue = GetValueFromReadLineText(214, 216, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.RESERVE = getValue;

                    // ===================================================
                    // 29. DESTINATION_CODE
                    // 216 - 222
                    // ===================================================
                    getValue = GetValueFromReadLineText(216, 222, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.DESTINATION_CODE = getValue;

                    // ===================================================
                    // 30. FACTORY_SHUKKA_NUMBER
                    // 222 - 237
                    // ===================================================
                    getValue = GetValueFromReadLineText(222, 237, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.FACTORY_SHUKKA_NUMBER = getValue;

                    // ===================================================
                    // 31. DELIVERY_NOTE_NUMBER
                    // 237 - 246
                    // ===================================================
                    getValue = GetValueFromReadLineText(237, 246, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.DELIVERY_NOTE_NUMBER = getValue;

                    // ===================================================
                    // 32. CREATED_DATE
                    // 246 - 252
                    // ===================================================
                    getValue = GetValueFromReadLineText(246, 252, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.CREATED_DATE = getValue;

                    // ===================================================
                    // 33. CREATED_TIME
                    // 252 - 258
                    // ===================================================
                    getValue = GetValueFromReadLineText(252, 258, line);
                    if (!string.IsNullOrEmpty(m_textDataErrorMessage))
                    {
                        obj.ERROR = m_textDataErrorMessage;
                    }
                    obj.CREATED_TIME = getValue;

                    // ===================================================
                    // 34. EXECUTE_RESULT                    
                    // ===================================================
                    obj.EXECUTE_RESULT = string.IsNullOrEmpty(obj.ERROR) ? 0 : 2;


                    // ===================================================
                    // 35. PROCESS_SERIAL_NUMBER                    
                    // ===================================================
                    // PROCESS_SERIAL_NUMBER
                    obj.PROCESS_SERIAL_NUMBER = rowIndex + 1;

                    // PROCESS_DATE_AND_TIME
                    
                   
                    obj.FUTOU_CD = futouCd;
                    

                    list.Add(obj);
                    rowIndex++;
                }
            }

            return Ok(new
            {
                isSuccess = true,
                data = list
            });
        }


        //private string GetValueFromReadLineText(int start, int end, string line)
        //{
        //    if (string.IsNullOrEmpty(line) || start < 0 || end <= start || start >= line.Length)
        //    {
        //        return string.Empty;
        //    }

        //    int length = end - start;

        //    if (start + length > line.Length)
        //    {
        //        return string.Empty;
        //    }

        //    return line.Substring(start, length).Trim();
        //}
        private string GetValueFromReadLineText(int start, int end, string line)
        {
            string value = string.Empty;

            try
            {
                m_textDataErrorMessage = string.Empty;

                if (start >= 0 && end > 0 && !string.IsNullOrEmpty(line))
                {
                    int length = end - start;

                    if (start + length <= line.Length)
                    {
                        value = line.Substring(start, length).Trim();
                    }
                    else
                    {
                        m_textDataErrorMessage = "NotFound";
                    }
                }
                else
                {
                    m_textDataErrorMessage = "NotFound";
                }
            }
            catch (Exception ex)
            {
                // Handle exception
            }

            return value;
        }


        private bool TryParseYYMMDD(string value, out DateTime parsedDate)
        {
            parsedDate = default;

            if (string.IsNullOrWhiteSpace(value) || value.Length != 6)
            {
                return false;
            }

            string yearPart = value.Substring(0, 2);
            string monthPart = value.Substring(2, 2);
            string dayPart = value.Substring(4, 2);

            if (!int.TryParse(yearPart, out int year) ||
                !int.TryParse(monthPart, out int month) ||
                !int.TryParse(dayPart, out int day))
            {
                return false;
            }

            // Assuming YY is in the range 2000-2099
            year += (year < 50 ? 2000 : 1900);

            return DateTime.TryParse($"{year}-{month:D2}-{day:D2}", out parsedDate);
        }
        
    }
}
