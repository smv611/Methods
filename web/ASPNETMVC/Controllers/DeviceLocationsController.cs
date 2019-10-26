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
    public class DeviceLocationsController : Controller
    {
        private MedTechEntities2 db = new MedTechEntities2();

        // GET: DeviceLocations
        public ActionResult Index(string searchString, int? page)
        {

            #region Default Code
            //var deviceLocations = db.DeviceLocations.Include(d => d.Device);
            //return View(deviceLocations.ToList());

            #endregion // end of Default Code

            #region LINQ Expression
            //var qData = from q in db.Devices.Include(d => d.DeviceDocument) select q;

            var qData = (from q in db.DeviceLocations.Include(d => d.Device) select q).OrderBy(s => s.FloorLoc).Skip(0);
            if (!string.IsNullOrEmpty(searchString))
            {
                qData = qData.Where(s => s.FloorLoc.Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var _viewModel = new List<DeviceLocationViewModel>();
            foreach (var item in qData)
            {
                _viewModel.Add(DeviceLocationViewModel.GetViewModelDatas(item));
            }

            return View("Index", _viewModel.ToPagedList(pageNumber, pageSize));

            //return View(_listViewModel.ToPagedList(pageNumber, pageSize));

            #endregion // end of LINQ Expression


            #region List
            //var _model = db.Devices.Include(d => d.DeviceDocument);
            //var _viewModel = new List<DeviceLocationViewModel>();

            //foreach (var item in _model)
            //{
            //    _viewModel.Add(DeviceLocationViewModel.GetViewModelDatas(item));
            //}

            //return View(_viewModel);
            #endregion // end of List 

        }
        //public ActionResult Details(DeviceLocationViewModel _viewModel)
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            //var _viewModel = db.DeviceLocations.Where(s => s.DeviceID == id).FirstOrDefault();
            //DeviceLocation _model = db.DeviceLocations.Where(s => s.DeviceID == id).FirstOrDefault();
            //DeviceLocation _model = db.DeviceLocations.Find(id);
            List<DeviceLocation> _model = db.DeviceLocations.Where(s => s.DeviceID == id).ToList();
            if (_model == null)
            {
                //return HttpNotFound();
                return View("ErrorView");

            }
            return View(_model);
        }

        public ActionResult Create()
        {
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeviceLocationViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                var _model = GetModelDatas(_viewModel);
                db.DeviceLocations.Add(_model);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _viewModel.DeviceID);
            return View(_viewModel);
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceLocation _model = db.DeviceLocations.Find(id);
            var _viewModel = DeviceLocationViewModel.GetViewModelDatas(_model);
            if (_model == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _model.DeviceID);
            return View(_viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DeviceLocationViewModel _viewModel)
        {
            if (ModelState.IsValid)
            {
                var _model = GetModelDatas(_viewModel);
                try
                {
                    db.Entry(_model).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    throw new DbEntityValidationException(ex.ToString());
                }
                return RedirectToAction("Index");
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _viewModel.DeviceID);
            return View(_viewModel);
        }

        DeviceLocation GetModelDatas(DeviceLocationViewModel _viewModel)
        {
            DeviceLocation _model = new DeviceLocation()
            {
                LocationID = _viewModel.LocationID,
                DeviceID = _viewModel.DeviceID,
                DepartmentLoc = _viewModel.DepartmentLoc.ToString(),
                CorpLoc = _viewModel.CorpLoc.ToString(),
                FloorLoc = _viewModel.FloorLoc.ToString(),
                RoomLoc = _viewModel.RoomLoc,
                NoteDevLoc = _viewModel.NoteDevLoc,
                CreateDateTime = _viewModel.CreateDateTime,
                ChangeDateTime = _viewModel.ChangeDateTime
            };
            return _model;
        }




        // GET: DeviceLocations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DeviceLocation deviceLocation = db.DeviceLocations.Find(id);
            if (deviceLocation == null)
            {
                return HttpNotFound();
            }
            return View(deviceLocation);
        }

        // POST: DeviceLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DeviceLocation deviceLocation = db.DeviceLocations.Find(id);
            db.DeviceLocations.Remove(deviceLocation);
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
