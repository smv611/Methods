using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
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
    public class ResponsiblePersonsController : Controller
    {
        private MedTechEntities2 db = new MedTechEntities2();

        // GET: ResponsiblePersons
        public ActionResult Index(string searchString, int? page)
        {
            #region DefaultCode
            //var responsiblePersons = db.ResponsiblePersons.Include(r => r.Device);
            //return View(responsiblePersons.ToList());

            #endregion End of DefaultCode

            #region List
            //var _model = db.ResponsiblePersons.Include(d => d.Device);
            //var _viewModel = new List<ResponsiblePersonViewModel>();

            //foreach (var item in _model)
            //{
            //    _viewModel.Add(ResponsiblePersonViewModel.GetViewModelDatas(item));
            //}

            //return View(_viewModel);
            #endregion end of List 

            #region LINQ Expression
            //var qData = from q in db.Devices.Include(d => d.DeviceDocument) select q;




            var qData = (from q in db.ResponsiblePersons.Include(d => d.Device) select q).OrderBy(s => s.DeviceID).Skip(0);
            if (!string.IsNullOrEmpty(searchString))
            {
                qData = qData.Where(s => s.SurnameRespPers.Contains(searchString));
            }

            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var _viewModel = new List<ResponsiblePersonViewModel>();
            foreach (var item in qData)
            {
                _viewModel.Add(ResponsiblePersonViewModel.GetViewModelDatas(item));
            }

            return View("Index", _viewModel.ToPagedList(pageNumber, pageSize));




            //return View(_listViewModel.ToPagedList(pageNumber, pageSize));

            #endregion // end of LINQ Expression

        }

        // GET: ResponsiblePersons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //ResponsiblePerson responsiblePerson = db.ResponsiblePersons.Find(id);
            List<ResponsiblePerson> _model = db.ResponsiblePersons.Where(s => s.DeviceID == id).ToList();

            if (_model == null)
            {
                //return HttpNotFound();
                return View("ErrorView");
            }
            return View(_model);
        }

        // GET: ResponsiblePersons/Create
        public ActionResult Create()
        {
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RespPersID,DeviceID,SurnameRespPers,NameRespPers,PatronymicRespPers,PersonNumberRespPers,PositionRespPers,DepartmentRespPers,CorpRespPers,FloorRespPers,RoomRespPers,PhoneRespPers,InternalPnoneRespPers,MobilePhoneRespPers,EmailRespPers,CreateDateTime,ChangeDateTime")] ResponsiblePerson responsiblePerson)
        {
            if (ModelState.IsValid)
            {
                db.ResponsiblePersons.Add(responsiblePerson);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", responsiblePerson.DeviceID);
            return View(responsiblePerson);
        }

        // GET: ResponsiblePersons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponsiblePerson _model = db.ResponsiblePersons.Find(id);
            var _viewModel = ResponsiblePersonViewModel.GetViewModelDatas(_model);
            if (_model == null)
            {
                return HttpNotFound();
            }
            ViewBag.DeviceID = new SelectList(db.Devices, "DeviceID", "InventoryNumber", _model.DeviceID);
            return View(_viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ResponsiblePersonViewModel _viewModel)
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

        ResponsiblePerson GetModelDatas(ResponsiblePersonViewModel _viewModel)
        {
            ResponsiblePerson _model = new ResponsiblePerson()
            {
                RespPersID = _viewModel.RespPersID,
                DeviceID = _viewModel.DeviceID,
                SurnameRespPers = _viewModel.SurnameRespPers,
                NameRespPers = _viewModel.NameRespPers,
                PatronymicRespPers = _viewModel.PatronymicRespPers,
                PersonNumberRespPers = _viewModel.PersonNumberRespPers,
                PositionRespPers = _viewModel.PositionRespPers.ToString(),
                DepartmentRespPers = _viewModel.DepartmentRespPers.ToString(),
                CorpRespPers = _viewModel.CorpRespPers.ToString(),
                FloorRespPers = _viewModel.FloorRespPers.ToString(),
                RoomRespPers = _viewModel.RoomRespPers,
                PhoneRespPers = _viewModel.PhoneRespPers,
                InternalPnoneRespPers = _viewModel.InternalPnoneRespPers,
                MobilePhoneRespPers = _viewModel.MobilePhoneRespPers,
                EmailRespPers = _viewModel.EmailRespPers,
                CreateDateTime = _viewModel.CreateDateTime,
                ChangeDateTime = _viewModel.ChangeDateTime
            };
            //_model.Device = _viewModel.Device;

            return _model;
        }














        // GET: ResponsiblePersons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ResponsiblePerson responsiblePerson = db.ResponsiblePersons.Find(id);
            if (responsiblePerson == null)
            {
                return HttpNotFound();
            }
            return View(responsiblePerson);
        }

        // POST: ResponsiblePersons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ResponsiblePerson responsiblePerson = db.ResponsiblePersons.Find(id);
            db.ResponsiblePersons.Remove(responsiblePerson);
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
