using System;
using ScalarValue = OpenFAST.ScalarValue;
using Scalar = OpenFAST.Template.Scalar;
using FASTType = OpenFAST.Template.Type.FASTType;

namespace OpenFAST.Template.operator_Renamed
{
	[Serializable]
	sealed class DefaultOperatorCodec:OperatorCodec
	{
		private const long serialVersionUID = 1L;
		
		internal DefaultOperatorCodec(Operator operator_Renamed, FASTType[] types):base(operator_Renamed, types)
		{
		}
		
		public override ScalarValue GetValueToEncode(ScalarValue value_Renamed, ScalarValue priorValue, Scalar field)
		{
			if (value_Renamed == null)
			{
				if (field.DefaultValue.Undefined)
					return null;
				return ScalarValue.NULL;
			}
			
			return value_Renamed.Equals(field.DefaultValue)?null:value_Renamed;
		}
		
		public override ScalarValue DecodeValue(ScalarValue newValue, ScalarValue previousValue, Scalar field)
		{
			return newValue;
		}
		
		public override ScalarValue DecodeEmptyValue(ScalarValue previousValue, Scalar field)
		{
			if (field.DefaultValue.Undefined)
				return null;
			return field.DefaultValue;
		}
		
		public  override bool Equals(System.Object obj)
		{
			return obj != null && obj.GetType() == GetType();
		}

        public override int GetHashCode()
		{
			return base.GetHashCode();
		}
	}
}