using UnityEngine;

namespace Spaceships.Resources {

    [CreateAssetMenu(fileName = "Integer Value", menuName = "Spaceships/Variables/int")]
    public class IntVariable : TypeVariable<int> {

        public override int Value { get; set; }

    }

}