//------------------------------------------------------------------------------
//  Copyright (c) 2009-2013 the original author or authors. All Rights Reserved. 
// 
//  NOTICE: You are permitted to use, modify, and distribute this file 
//  in accordance with the terms of the license agreement accompanying it. 
//------------------------------------------------------------------------------
using System.Collections.Generic;
using System;

namespace robotlegs.bender.extensions.matching
{
	/**
	 * A filter that describes a package matcher
	 */
	public class NamespaceFilter : ITypeFilter
	{

		/*============================================================================*/
		/* Public Properties                                                          */
		/*============================================================================*/

		protected string _descriptor;

		/**
		 * @inheritDoc
		 */
		public string Descriptor
		{
			get
			{
				if (_descriptor == null)
				{
					_descriptor = CreateDescriptor ();
				}
				return _descriptor;
			}
		}

		public List<Type> AllOfTypes
		{
			get
			{
				return emptyVector;
			}
		}

		public List<Type> AnyOfTypes
		{
			get
			{
				return emptyVector;
			}
		}

		public List<Type> NoneOfTypes
		{
			get
			{
				return emptyVector;
			}
		}

		/*============================================================================*/
		/* Protected Properties                                                       */
		/*============================================================================*/

		protected List<Type> emptyVector = new List<Type>();

		protected string _requirePackage;

		protected List<string> _anyOfPackages;

		protected List<string> _noneOfPackages;

		/*============================================================================*/
		/* Constructor                                                                */
		/*============================================================================*/

		/**
		 * Creates a new Package Filter
		 * @param requiredPackage
		 * @param anyOfPackages
		 * @param noneOfPackages
		 */
		public NamespaceFilter(
			string requiredPackage,
			List<string> anyOfPackages,
			List<string> noneOfPackages)
		{
			_requirePackage = requiredPackage;
			_anyOfPackages = anyOfPackages;
			_noneOfPackages = noneOfPackages;
			_anyOfPackages.Sort();
			_noneOfPackages.Sort();
		}

		/*============================================================================*/
		/* Public Functions                                                           */
		/*============================================================================*/

		/**
		 * @inheritDoc
		 */
		public bool Matches(object item)
		{
			Type itemType = item is Type ? item as Type : item.GetType ();
			string itemNamespace = itemType.Namespace;

			if (_requirePackage != null && (!MatchPackageInNamespace(_requirePackage, itemNamespace)))
				return false;

			foreach (string packageName in _noneOfPackages)
			{
				if (MatchPackageInNamespace(packageName, itemNamespace))
					return false;
			}

			foreach (string packageName in _anyOfPackages)
			{
				if (MatchPackageInNamespace(packageName, itemNamespace))
					return true;
			}
			if (_anyOfPackages.Count > 0)
				return false;

			if (_requirePackage != null)
				return true;

			if (_noneOfPackages.Count > 0)
				return true;

			return false;
		}

		/*============================================================================*/
		/* Private Functions                                                          */
		/*============================================================================*/

		private string CreateDescriptor()
		{
			return "require: " + _requirePackage
				+ ", any of: " + ListToString (_anyOfPackages)
				+ ", none of: " + ListToString (_noneOfPackages);
		}

		private bool MatchPackageInNamespace(string packageName, string nmspace)
		{
			return (nmspace.IndexOf (packageName) == 0);
		}

		private string ListToString(List<string> list)
		{
			string returnString = "";
			int listLength = list.Count;
			for (int i = 0; i < listLength; i++)
			{
				returnString += list[i];
				if (i < listLength - 1)
				{
					returnString += ",";
				}
			}
			return returnString;
		}
	}
}
