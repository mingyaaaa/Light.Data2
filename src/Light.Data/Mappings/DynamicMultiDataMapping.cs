﻿using System;
using System.Collections.Generic;
using System.Data;

namespace Light.Data
{
	internal class DynamicMultiDataMapping : DataMapping
	{
		public static DynamicMultiDataMapping CreateDynamicMultiDataMapping (Type type, List<IJoinModel> models)
		{
			var array = new Tuple<string, IJoinTableMapping> [models.Count];
			for (var i = 0; i < models.Count; i++) {
				var model = models [i];
				var tuple = new Tuple<string, IJoinTableMapping> (model.AliasTableName, model.JoinMapping);
				array [i] = tuple;
			}
			var mapping = new DynamicMultiDataMapping (type, array);
			return mapping;
		}

		private readonly IJoinTableMapping [] mappings;
		private readonly string [] aliasNames;

		public DynamicMultiDataMapping (Type type, Tuple<string, IJoinTableMapping> [] targetMappings)
			: base (type)
		{
			mappings = new IJoinTableMapping [targetMappings.Length];
			aliasNames = new string [targetMappings.Length];
			for (var i = 0; i < targetMappings.Length; i++) {
				aliasNames [i] = targetMappings [i].Item1;
				mappings [i] = targetMappings [i].Item2;
			}
		}

		public override object InitialData ()
		{
			var objects = new object [mappings.Length];
			for (var i = 0; i < mappings.Length; i++) {
				objects [i] = mappings [i].InitialData ();
			}
			return objects;
		}

		public override object LoadData (DataContext context, IDataReader datareader, object state)
		{
			var queryState = state as QueryState;
			var objects = new object [mappings.Length];
			for (var i = 0; i < mappings.Length; i++) {
				var aliasName = aliasNames [i];
				objects [i] = mappings [i].LoadAliasJoinTableData (context, datareader, queryState, aliasName);
			}
			return objects;
		}
	}
}

