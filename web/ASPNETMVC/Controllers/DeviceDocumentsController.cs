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
using System.Data.Entity.Validation;

namespace WebAppMedTechTest.Controllers
{
    public class DeviceDocumentsController : Controller
    {
        private MedTechEntities2 db = new MedTechEntities2();

        public ActionResult Index()
        {
            #region MyRegion
            //var deviceDocuments = db.DeviceDocuments.Include(d => d.Device);
            //return View(deviceDocuments.ToList());

            #endregion // end of MyRegion 

            #region List
            var _model = db.DeviceDocuments.Include(d => d.Device);
            var _viewModel = new List<DeviceDocumentViewModel>();

            foreach (var item in _model)
            {
                _viewModel.Add(DeviceDocumentViewModel.GetViewModelDatas(item));
            }

            return View(_viewModel);
            #endregion // end of List 
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceDocument deviceDocument = db.DeviceDocuments.Find(id);
            if (deviceDocument == null)
            {
                //return HttpNotFound();
                return View("ErrorView");

            }
            return View(deviceDocument);
        }

        public ActionResult Create()
        {
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeviceDocument _model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.DeviceDocuments.Add(_model);
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    //ViewBag.Error = ex.ToString();
                    throw new DbEntityValidationException(ex.ToString());
                    //return View("SaveError");
                }

                return RedirectToAction("Index");
            }

            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _model.DeviceID);
            return View(_model);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceDocument _model = db.DeviceDocuments.Find(id);
            var _viewModel = DeviceDocumentViewModel.GetViewModelDatas(_model);
            if (_model == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _model.DeviceID);
            return View(_viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceDocumentViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                DeviceDocument _model = GetModelDatas(_viewModel);
                db.Entry(_model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "DeviceType", _viewModel.DeviceID);
            return View(_viewModel);
        }

        DeviceDocument GetModelDatas(DeviceDocumentViewModel _viewModel)
        {
            DeviceDocument _model = new DeviceDocument()
            {
                DeviceID = _viewModel.DeviceID,
                ActPuttingOperationFileName = _viewModel.ActPuttingOperationFileName,
                ActPuttingOperationBinData = _viewModel.ActPuttingOperationBinData,
                RegistrationCertificateFileName = _viewModel.RegistrationCertificateFileName,
                RegistrationCertificateBinData = _viewModel.RegistrationCertificateBinData,
                CertificateConformityDeviceFileName = _viewModel.CertificateConformityDeviceFileName,
                CertificateConformityDeviceBinData = _viewModel.CertificateConformityDeviceBinData,
                Photo1FileName = _viewModel.Photo1FileName,
                Photo1BinData = _viewModel.Photo1BinData,
                Photo2FileName = _viewModel.Photo2FileName,
                Photo2BinData = _viewModel.Photo2BinData,
                Photo3FileName = _viewModel.Photo3FileName,
                Photo3BinData = _viewModel.Photo3BinData,
                ServiceManualFileName = _viewModel.ServiceManualFileName,
                ServiceManualBinData = _viewModel.ServiceManualBinData,
                UserManualFileName = _viewModel.UserManualFileName,
                UserManualBinData = _viewModel.UserManualBinData,
                PassportDeviceFileName = _viewModel.PassportDeviceFileName,
                PassportDeviceBinData = _viewModel.PassportDeviceBinData,
                CreateDateTime = _viewModel.CreateDateTime,
                ChangeDateTime = _viewModel.ChangeDateTime
            };
            _viewModel.Device = new Device();
            _model.Device = _viewModel.Device;

            return _model;
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceDocument deviceDocument = db.DeviceDocuments.Find(id);
            if (deviceDocument == null)
            {
                return HttpNotFound();
            }
            return View(deviceDocument);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceDocument deviceDocument = db.DeviceDocuments.Find(id);
            db.DeviceDocuments.Remove(deviceDocument);
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
