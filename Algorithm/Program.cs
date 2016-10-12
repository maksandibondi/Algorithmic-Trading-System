using System;

namespace Algorithm {

	public interface IfileAlgorithm {

		string run();
			
	}

	public class FileAlgorithm : IfileAlgorithm {

		public string run() {

			string x = "some simle algorithm that returns string";

			return x;

		}

	}

}