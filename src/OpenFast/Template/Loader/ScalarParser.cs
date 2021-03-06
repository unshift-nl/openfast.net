/*

The contents of this file are subject to the Mozilla Public License
Version 1.1 (the "License"); you may not use this file except in
compliance with the License. You may obtain a copy of the License at
http://www.mozilla.org/MPL/

Software distributed under the License is distributed on an "AS IS"
basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. See the
License for the specific language governing rights and limitations
under the License.

The Original Code is OpenFAST.

The Initial Developer of the Original Code is The LaSalle Technology
Group, LLC.  Portions created by Shariq Muhammad
are Copyright (C) Shariq Muhammad. All Rights Reserved.

Contributor(s): Shariq Muhammad <shariq.muhammad@gmail.com>
                Yuri Astrakhan <FirstName><LastName>@gmail.com
*/
using System.Xml;
using OpenFAST.Error;
using OpenFAST.Template.Operators;
using OpenFAST.Template.Types;
using OpenFAST.Utility;

namespace OpenFAST.Template.Loader
{
    public class ScalarParser : AbstractFieldParser
    {
        public ScalarParser(string[] nodeNames) : base(nodeNames)
        {
        }

        public ScalarParser(string nodeName) : base(nodeName)
        {
        }

        public ScalarParser() : base(new string[] {})
        {
        }

        public override bool CanParse(XmlElement element, ParsingContext context)
        {
            return context.TypeMap.ContainsKey(GetTypeName(element));
        }

        public override Field Parse(XmlElement fieldNode, bool optional, ParsingContext context)
        {
            Operator op = Operator.None;
            string defaultValue = null;
            string key = null;
            string ns = "";
            XmlElement operatorElement = GetOperatorElement(fieldNode);
            if (operatorElement != null)
            {
                if (operatorElement.HasAttribute("value"))
                    defaultValue = operatorElement.GetAttribute("value");
                op = Operator.GetOperator(operatorElement.LocalName);
                if (operatorElement.HasAttribute("key"))
                    key = operatorElement.GetAttribute("key");
                if (operatorElement.HasAttribute("ns"))
                    ns = operatorElement.GetAttribute("ns");
                if (operatorElement.HasAttribute("dictionary"))
                    context.Dictionary = operatorElement.GetAttribute("dictionary");
            }

            FastType type = GetType(fieldNode, context);
            var scalar = new Scalar(GetName(fieldNode, context), type, op, type.GetValue(defaultValue),
                                    optional);
            if (fieldNode.HasAttribute("id"))
                scalar.Id = fieldNode.GetAttribute("id");
            if (key != null)
                scalar.Key = new QName(key, ns);
            scalar.Dictionary = context.Dictionary;
            ParseExternalAttributes(fieldNode, scalar);
            return scalar;
        }

        protected virtual QName GetName(XmlElement fieldNode, ParsingContext context)
        {
            return context.Name;
        }

        protected virtual FastType GetType(XmlElement fieldNode, ParsingContext context)
        {
            string typeName = GetTypeName(fieldNode);
            FastType value;
            if (!context.TypeMap.TryGetValue(typeName, out value))
            {
                context.ErrorHandler.OnError(
                    null, StaticError.InvalidType,
                    "The type {0} is not defined.  Possible types: {1}", typeName,
                    Util.CollectionToString(context.TypeMap.Keys, ", "));
            }
            return value;
        }

        protected virtual string GetTypeName(XmlElement fieldNode)
        {
            return fieldNode.LocalName;
        }

        protected virtual XmlElement GetOperatorElement(XmlElement fieldNode)
        {
            return GetElement(fieldNode, 1);
        }
    }
}