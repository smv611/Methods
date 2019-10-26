using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebAppMedTechTest.Models;
using WebAppMedTechTest.ViewModel;
using PagedList;
using PagedList.Mvc;
using System.Data.Entity.Validation;


namespace WebAppMedTechTest.Controllers
{
    public class TreatiesController : Controller
    {
        private MedTechEntities2 db = new MedTechEntities2();

        // GET: Treaties
        public ActionResult Index(string searchString, int? page)
        {
            #region Default Code
            //var treaties = db.Treaties.Include(t => t.Contractor);
            //return View(treaties.ToList());

            #endregion End of Default Code

            #region List
            //var _model = db.Treaties.Include(d => d.Contractor);
            //var _viewModel = new List<TreatyViewModel>();

            //foreach (var item in _model)
            //{
            //    _viewModel.Add(TreatyViewModel.GetViewModelDatas(item));
            //}

            //return View(_viewModel);
            #endregion // end of List 

            #region LINQ Expression
            //var qData = from q in db.Devices.Include(d => d.DeviceDocument) select q;

            var qData = (from q in db.Treaties.Include(d => d.Contractor) select q).OrderBy(s => s.TreatyNumber).Skip(0);
            if (!string.IsNullOrEmpty(searchString))
            {
                qData = qData.Where(s => s.TreatyNumber.Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var _viewModel = new List<TreatyViewModel>();
            foreach (var item in qData)
            {
                _viewModel.Add(TreatyViewModel.GetViewModelDatas(item));
            }

            return View("Index", _viewModel.ToPagedList(pageNumber, pageSize));

            //return View(_listViewModel.ToPagedList(pageNumber, pageSize));

            #endregion // end of LINQ Expression



        }

        // GET: Treaties/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Treaty treaty = db.Treaties.Find(id);
            List<Treaty> _model = db.Treaties.Where(s => s.ContractorID == id).ToList();

            if (_model == null)
            {
                //return HttpNotFound();
                return View("ErrorView");
            }
            return View(_model);
        }

        public ActionResult Create()
        {
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Company");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TreatyViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                Treaty _model = new Treaty();
                _model = GetModelDatas(_viewModel);
                db.Treaties.Add(_model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Company", _viewModel.ContractorID);
            return View(_viewModel);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treaty _model = db.Treaties.Find(id);
            var _viewModel = TreatyViewModel.GetViewModelDatas(_model);
            if (_model == null)
            {
                return HttpNotFound();
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Company", _model.ContractorID);
            return View(_viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TreatyViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                var _model = GetModelDatas(_viewModel);
                db.Entry(_model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ContractorID = new SelectList(db.Contractors, "ContractorID", "Company", _viewModel.ContractorID);
            return View(_viewModel);
        }

        Treaty GetModelDatas(TreatyViewModel _viewModel) {
            Treaty _model = new Treaty()
            {
                TreatyID = _viewModel.TreatyID,
                ContractorID = _viewModel.ContractorID,
                TreatyNumber = _viewModel.TreatyNumber,
                TreatyFileName = _viewModel.TreatyFileName,
                TreatyBinData = _viewModel.TreatyBinData,
                SurnameTreatyPers = _viewModel.SurnameTreatyPers,
                NameTreatyPers = _viewModel.NameTreatyPers,
                PatronymicTreatyPers = _viewModel.PatronymicTreatyPers,
                PositionTreatyPers = _viewModel.PositionTreatyPers,
                DepartmentTreatyPers = _viewModel.DepartmentTreatyPers,
                PhoneTreatyPers = _viewModel.PhoneTreatyPers,
                InternalPnoneTreatyPers = _viewModel.InternalPnoneTreatyPers,
                MobilePhoneTreatyPers = _viewModel.MobilePhoneTreatyPers,
                EmailTreatyPers = _viewModel.EmailTreatyPers,
                AgreementTerm = _viewModel.AgreementTerm,
                CommencementAgreement = _viewModel.CommencementAgreement,
                TerminationAgreement = _viewModel.TerminationAgreement,
                CostAgreement = _viewModel.CostAgreement,
                PriceContractMonth = _viewModel.PriceContractMonth,
                CertificateCompletionFileName = _viewModel.CertificateCompletionFileName,
                CertificateCompletionBinData = _viewModel.CertificateCompletionBinData,
                CreateDateTime = _viewModel.CreateDateTime,
                ChangeDateTime = _viewModel.ChangeDateTime,
                Contractor = _viewModel.Contractor
            };
            return _model;


        }





        // GET: Treaties/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Treaty treaty = db.Treaties.Find(id);
            if (treaty == null)
            {
                return HttpNotFound();
            }
            return View(treaty);
        }

        // POST: Treaties/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Treaty treaty = db.Treaties.Find(id);
            db.Treaties.Remove(treaty);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
