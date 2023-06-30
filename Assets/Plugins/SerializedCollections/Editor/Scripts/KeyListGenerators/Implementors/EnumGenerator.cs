using System.Collections;

namespace Assets.Plugins.SerializedCollections.Editor.Scripts.KeyListGenerators.Implementors
{
    [KeyListGenerator("Populate Enum", typeof(System.Enum), false)]
    public class EnumGenerator : KeyListGenerator
    {
        public override IEnumerable GetKeys(System.Type type)
        {
            return System.Enum.GetValues(type);
        }
    }
}