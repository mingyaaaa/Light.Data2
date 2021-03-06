﻿namespace Light.Data
{
	internal class DataFieldOrderExpression : OrderExpression
	{
		private readonly DataFieldInfo _fieldInfo;

		private readonly OrderType _orderType;

		public DataFieldOrderExpression (DataFieldInfo fieldInfo, OrderType orderType)
			: base (fieldInfo.TableMapping)
		{
			_fieldInfo = fieldInfo;
			_orderType = orderType;
		}

		internal override string CreateSqlString (CommandFactory factory, bool isFullName, CreateSqlState state)
		{
			var fieldSql = _fieldInfo.CreateSqlString (factory, isFullName, state);
			return factory.CreateOrderBySql (fieldSql, _orderType);
		}

		internal override OrderExpression CreateAliasTableNameOrder (string aliasTableName)
		{
			var info = _fieldInfo.CreateAliasTableInfo (aliasTableName);
			return new DataFieldOrderExpression (info, _orderType);
		}
	}
}
