using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using HentaiViewer.Common;
using HentaiViewer.Models;

namespace HentaiViewer.ViewModels {
	public class FavoritesViewModel {
		public ObservableCollection<HentaiModel> _favorites = new ObservableCollection<HentaiModel>();
		public ReadOnlyObservableCollection<HentaiModel> FavoriteItems { get; }

		public static FavoritesViewModel Instance;

		public FavoritesViewModel() {
			Instance = this;
			FavoriteItems = new ReadOnlyObservableCollection<HentaiModel>(_favorites);
			FavoritesController.Favorites.ForEach(_favorites.Add);
		}
	}
}
