using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Echo.State;
using Echo.Web.Areas.Admin.Models;

namespace Echo.Web.Areas.Admin.Controllers
{
    public class StarClusterController : Controller
    {
	    private readonly IBackingStore<StarClusterState> _backingStore;

	    public StarClusterController(IBackingStore<StarClusterState> backingStore)
	    {
		    _backingStore = backingStore;
	    }

	    public ActionResult Index()
	    {
			var model = new StarClusterIndex() { StarClusters = _backingStore.GetAll(), NewStarCluster = new StarCluster() };
		    return View(model);
	    }

		public ActionResult Add(StarCluster newStarCluster)
		{
			if (ModelState.IsValid)
			{
				_backingStore.Add(new StarClusterState { LocalCoordinates = newStarCluster.LocalCoordinates, Name = newStarCluster.Name });

				return RedirectToAction("Index");
			}

			var model = new StarClusterIndex() { StarClusters = _backingStore.GetAll(), NewStarCluster = newStarCluster };
			return View("Index", model);
		}

	    public ActionResult View()
	    {
		    throw new NotImplementedException();
	    }
    }

	public interface IBackingStore<T>
	{
		IEnumerable<T> GetAll();
		void Add(T value);
	}
}
