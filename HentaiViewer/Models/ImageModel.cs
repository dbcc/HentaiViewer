using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PropertyChanged;

namespace HentaiViewer.Models {
	[ImplementPropertyChanged]
	public class ImageModel {
		public int PageNumber { get; set; }
		public object Source { get; set; }
	}
}
