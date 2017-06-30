﻿/*

	This file is part of KeywordMacroscope.

	Copyright 2017 Jason Holland.

	The GitHub repository may be found at:

		https://github.com/nazuke/KeywordMacroscope

	Foobar is free software: you can redistribute it and/or modify
	it under the terms of the GNU General Public License as published by
	the Free Software Foundation, either version 3 of the License, or
	(at your option) any later version.

	Foobar is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
	GNU General Public License for more details.

	You should have received a copy of the GNU General Public License
	along with Foobar.  If not, see <http://www.gnu.org/licenses/>.

*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using HtmlAgilityPack;

namespace KeywordMacroscope
{

	/// <summary>
	/// Description of MacroscopeSerps.
	/// </summary>

	public class MacroscopeSerps : MacroscopeBase
	{

		/**************************************************************************/

		/**************************************************************************/

		public MacroscopeSerps ()
		{
		}

		/**************************************************************************/

		private HtmlDocument FetchSearchResultsPage( string Url )
		{

			HtmlDocument doc = new HtmlDocument ();




			return( doc );

		}

		/**************************************************************************/

		private MacroscopeTrackedKeywordList ExtractTrackedKeywordsList( HtmlDocument doc )
		{

			MacroscopeTrackedKeywordList TrackedKeywordList = new MacroscopeTrackedKeywordList ();




			return( TrackedKeywordList );
			
		}

		/**************************************************************************/

	}

}
