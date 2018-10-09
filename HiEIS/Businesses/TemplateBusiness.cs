using AutoMapper;
using HiEIS.Areas.Public.Models;
using HiEIS.Entities;
using HiEIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;

namespace HiEIS.Businesses
{
    public class TemplateBusiness
    {
        public List<TemplateViewModel> GetAllTemplates(int companyId)
        {
            using (var db = new HiEISEntities())
            {
                var templates = db.Templates
                                    .Where(p => p.CompanyId == companyId)
                                    .Select(Mapper.Map<Template, TemplateViewModel>)
                                    .ToList();
                return templates;
            }

        }

        public bool AddNewTemplate(CreateTemplateModel model, int companyId)
        {
            bool result;
            using (var db = new HiEISEntities())
            {
                try
                {
                    model.CompanyId = companyId;
                    model.CurrentNo = model.BeginNo - 1;
                    model.ReleaseDate = DateTime.ParseExact(model.Date, "dd/MM/yyyy", null);

                    var template = Mapper.Map<CreateTemplateModel, Template>(model);
                    db.Templates.Add(template);
                    db.SaveChanges();

                    result = true;
                }
                catch (Exception)
                {
                   
                    result = false;
                }
            }
            return result;
        }

        public bool AddMoreBlock(UpdateTemplateModel model, int companyId)
        {
            using (var db = new HiEISEntities())
            {
                var originalTemplate = GetTemplateById(model.Id);
                var newTemplate = new CreateTemplateModel();
                var currentNumber = originalTemplate.Amount + originalTemplate.BeginNo - 1;

                if (originalTemplate.CurrentNo == currentNumber)
                {
                    newTemplate.IsActive = true;
                }
                else
                {
                    newTemplate.IsActive = false;
                }
                
                newTemplate.Name = model.Name;
                newTemplate.Form = model.Form;
                newTemplate.Serial = model.Serial;
                newTemplate.FileUrl = originalTemplate.FileUrl;
                newTemplate.Amount = model.Amount;
                newTemplate.BeginNo = currentNumber + 1;
                newTemplate.ReleaseAnnouncementUrl = model.ReleaseAnnouncementUrl;
                newTemplate.Date = model.Date;

                bool result = AddNewTemplate(newTemplate, companyId);
                return result;
            }
        }

        public UpdateTemplateModel GetTemplateById(int id)
        {
            using (var db = new HiEISEntities())
            {
                var templateInDb = db.Templates.Find(id);
                var template = new UpdateTemplateModel();
                if (templateInDb != null)
                {
                    template.CompanyId = templateInDb.CompanyId;
                    template.Id = templateInDb.Id;
                    template.Name = templateInDb.Name;
                    template.Form = templateInDb.Form;
                    template.Serial = templateInDb.Serial;
                    template.FileUrl = templateInDb.FileUrl;
                    template.Amount = templateInDb.Amount;
                    template.BeginNo = templateInDb.BeginNo;
                    template.CurrentNo = templateInDb.CurrentNo;
                    template.ReleaseDate = templateInDb.ReleaseDate;
                    template.ReleaseAnnouncementUrl = templateInDb.ReleaseAnnouncementUrl;
                    template.Date = null;
                    template.ReleaseAnnounmentFile = null;
                    template.IsActive = templateInDb.IsActive;
                    return template;
                }
                return null;
            }
        }

        public void UpdateTemplate(TemplateViewModel model, int id)
        {
            using (var db = new HiEISEntities())
            {
                var templateInDb = db.Templates.Find(id);
                if (templateInDb == null)
                {
                    throw new HttpResponseException(HttpStatusCode.NotFound);
                }
                Mapper.Map(model, templateInDb);
                db.SaveChanges();
            }
        }

        public List<TemplateViewModel> GetTemplateIsActive(int companyId)
        {
            using (var db = new HiEISEntities())
            {

                var templates = db.Templates
                                    .Where(a => a.CompanyId == companyId
                                                && a.IsActive == true)
                                    .Select(Mapper.Map<Template, TemplateViewModel>)
                                    .ToList();
                return templates;
            }
        }

        public TemplateViewModel GetTemplateByFormSerialActive(int companyId, string form, string serial)
        {
            using (var db = new HiEISEntities())
            {
                var template = db.Templates
                                    .FirstOrDefault(a => a.CompanyId == companyId
                                                && a.Form == form.ToUpper()
                                                && a.Serial == serial.ToUpper()
                                                && a.IsActive == true);

                return Mapper.Map<Template, TemplateViewModel>(template);
            }
        }

        public int GetCompanyId(int templateId)
        {
            using (var db = new HiEISEntities())
            {
                var templateInDb = db.Templates.Find(templateId);
                return templateInDb.CompanyId;
            }
        }

        public List<TemplateViewModel> GetTemplateActiveByCompanyId(int companyId)
        {
            using (var db = new HiEISEntities())
            {
                var templates = db.Templates
                                    .Where(p => p.CompanyId == companyId
                                                && p.IsActive == true)
                                    .Select(Mapper.Map<Template, TemplateViewModel>)
                                    .ToList();
                return templates;
            }
        }

        public DataTableResponseModel<TemplateViewModel> GetTemplatesByFormSerial(
            int templateId
            //,int companyId
            , DataTableRequestModel req
            )
        {
            using (var db = new HiEISEntities())
            {
                var originTemplate = db.Templates.Find(templateId);
                //var originTemplate = db.Templates.FirstOrDefault(a => a.Id == templateId && a.CompanyId == companyId);
                var templates = db.Templates
                                    .Where(a => a.CompanyId == originTemplate.CompanyId
                                                && a.Form == originTemplate.Form.ToUpper()
                                                && a.Serial == originTemplate.Serial.ToUpper());

                var pagingResult = templates.OrderByDescending(a => a.Id)
                                    .Skip(req.page * req.pageSize)
                                    .Take(req.pageSize)
                                    .Select(Mapper.Map<Template, TemplateViewModel>)
                                    .ToList();

                return new DataTableResponseModel<TemplateViewModel>
                {
                    data = pagingResult,
                    display = templates.Count(),
                    total = templates.Count()
                };
            }
        }

        public List<TemplateViewModel> GetTemplateByFormSerial(int companyId, string form, string serial)
        {
            using (var db = new HiEISEntities())
            {
                var template = db.Templates
                                    .Where(a => a.CompanyId == companyId
                                                && a.Form == form.ToUpper()
                                                && a.Serial == serial.ToUpper())
                                    .Select(Mapper.Map<Template, TemplateViewModel>)
                                    .ToList();

                return template;
            }
        }

        //        select distinct Form, Serial
        //from Template
        //where CompanyId = '1'

        public void UpdateNumber(int templateId, int companyId, HiEISEntities db)
        {
            var t = db.Templates.Find(templateId);

            t.CurrentNo = t.CurrentNo + 1;
            var lastNo = t.Amount + t.BeginNo - 1;

            if (t.CurrentNo == lastNo)
            {
                t.IsActive = false;
                var nextBlock = GetNextTemplateBlock(t, companyId, db);
                nextBlock.IsActive = true;
            }
            db.SaveChanges();
        }
        
        public Template GetNextTemplateBlock(Template currentTemplate, int companyId, HiEISEntities db)
        {
            var nextBlock = db.Templates
                                .FirstOrDefault(a => a.CompanyId == companyId
                                            && a.Form == currentTemplate.Form.ToUpper()
                                            && a.Serial == currentTemplate.Serial.ToUpper()
                                            && a.BeginNo == currentTemplate.CurrentNo + 1);
            return nextBlock;
        }

        public bool DeleteTemplate(int id, int companyId)
        {
            bool result = false;
            using (var db = new HiEISEntities())
            {
                try
                {
                    var templateInDb = db.Templates.Find(id);
                    if (templateInDb != null &&
                        templateInDb.CompanyId == companyId)
                    {
                        db.Templates.Remove(templateInDb);
                        db.SaveChanges();
                        result = true;
                    }
                }
                catch (Exception)
                {
                    result = false;
                    throw;
                }
                
            }
            return result;
        }
    }
}