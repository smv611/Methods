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



namespace WebAppMedTechTest.Controllers
{
    public class DevicesController : Controller
    {
        private MedTechEntities2 db = new MedTechEntities2();

        // GET: Devices
        public ActionResult Index(string searchString, int? page)
        {

            #region DefaultCode
            //var devices = db.Devices.Include(d => d.DeviceDocument);
            //return View(devices.ToList());

            #endregion // end of DefaultCode


            #region LINQ Expression
            //var qData = from q in db.Devices.Include(d => d.DeviceDocument) select q;

            var qData = (from q in db.Devices.Include(d => d.DeviceDocument) select q).OrderBy(s => s.Vendor).Skip(0);
            if (!string.IsNullOrEmpty(searchString))
            {
                qData = qData.Where(s => s.DeviceType.Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var _viewModel = new List<DevicesViewModel>();
            foreach (var item in qData)
            {
                _viewModel.Add(DevicesViewModel.GetViewModelDatas(item));
            }

            return View("Index", _viewModel.ToPagedList(pageNumber, pageSize));
            //return View(_listViewModel.ToPagedList(pageNumber, pageSize));

            #endregion // end of LINQ Expression

            #region List
            //var _model = db.Devices.Include(d => d.DeviceDocument);
            //var _viewModel = new List<DevicesViewModel>();

            //foreach (var item in _model)
            //{
            //    _viewModel.Add(DevicesViewModel.GetViewModelDatas(item));
            //}

            //return View(_viewModel);
            #endregion // end of List 
        }

        // GET: Devices/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //Device device = db.Devices.Find(id);
            Device device = db.Devices.Where(s=>s.DeviceID == id).FirstOrDefault();
            if (device == null)
            {
                //return HttpNotFound();
                return View("ErrorView");
            }
            return View(device);
        }

        // GET: Devices/Create
        public ActionResult Create()
        {
            ViewBag.DeviceID = new SelectList(db.DeviceDocuments, "DeviceID", "ActPuttingOperationFileName");
            return View();
        }

        // POST: Devices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DevicesViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                Device _model = new Device();

                #region CheckForNull
                if (_viewModel.DeviceType==0)
                {
                    _model.DeviceType=null;
                }
                else
                {
                    _model.DeviceType = _viewModel.DeviceType.ToString();
                }

                if (_viewModel.Vendor == 0)
                {
                    _model.Vendor = null;
                }
                else
                {
                    _model.Vendor = _viewModel.Vendor.ToString();
                }

                if (string.IsNullOrEmpty(_viewModel.Model))
                {
                    _model.Model = "";
                }
                else
                {
                    _model.Model = _viewModel.Model.ToString();
                }

                if (string.IsNullOrEmpty(_viewModel.VendorArticle))
                {
                    _model.VendorArticle = "";
                }
                else
                {
                    _model.VendorArticle = _viewModel.VendorArticle.ToString();
                }

                if (string.IsNullOrEmpty(_viewModel.SerialNumber))
                {
                    _model.SerialNumber = "";
                }
                else
                {
                    _model.SerialNumber = _viewModel.SerialNumber.ToString();
                }

                if (string.IsNullOrEmpty(_viewModel.InventoryNumber))
                {
                    _model.InventoryNumber = "";
                }
                else
                {
                    _model.InventoryNumber = _viewModel.InventoryNumber.ToString();
                }

                if (string.IsNullOrEmpty(_viewModel.NoteDevice))
                {
                    _model.NoteDevice = "";
                }
                else
                {
                    _model.NoteDevice = _viewModel.NoteDevice.ToString();
                }

                if (string.IsNullOrEmpty(_viewModel.Model))
                {
                    _model.Model = "";
                }
                else
                {
                    _model.Model = _viewModel.Model.ToString();
                }


                #endregion // end CheckForNull

                db.Devices.Add(_model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeviceID = new SelectList(db.DeviceDocuments, "DeviceID", "ActPuttingOperationFileName", _viewModel.DeviceID);
            return View(_viewModel);
        }

        // GET: Devices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device _model = db.Devices.Find(id);
            var _viewModel = DevicesViewModel.GetViewModelDatas(_model);
            if (_model == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceID = new SelectList(db.DeviceDocuments, "DeviceID", "ActPuttingOperationFileName", _model.DeviceID);
            return View(_viewModel);
        }

        // POST: Devices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DevicesViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                Device _model = new Device()
                {
                    DeviceID = _viewModel.DeviceID,
                    DeviceType = _viewModel.DeviceType.ToString(),
                    Vendor = _viewModel.Vendor.ToString(),
                    Model = _viewModel.Model,
                    VendorArticle = _viewModel.VendorArticle,
                    SerialNumber = _viewModel.SerialNumber,
                    InventoryNumber = _viewModel.InventoryNumber,
                    NoteDevice = _viewModel.NoteDevice,
                    ChangeDateTime = _viewModel.ChangeDateTime
                };
                db.Entry(_model).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.DeviceID = new SelectList(db.DeviceDocuments, "DeviceID", "ActPuttingOperationFileName", _viewModel.DeviceID);
            return View(_viewModel);
        }

        // GET: Devices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Device device = db.Devices.Find(id);
            if (device == null)
            {
                return HttpNotFound();
            }
            return View(device);
        }

        // POST: Devices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Device device = db.Devices.Find(id);
            db.Devices.Remove(device);
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
