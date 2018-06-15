// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotnetSpider.HtmlAgilityPack
{
    // Adapted from Mono Rocks

    internal abstract class Either<TA, TB>
            : IEquatable<Either<TA, TB>>
    {
        private Either() { }

        public static Either<TA, TB> A(TA value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return new AImpl(value);
        }

        public static Either<TA, TB> B(TB value)
        {
            if (value == null) throw new ArgumentNullException("value");
            return new BImpl(value);
        }

        public abstract override bool Equals(object obj);
        public abstract bool Equals(Either<TA, TB> obj);
        public abstract override int GetHashCode();
        public abstract override string ToString();
        public abstract TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b);

        private sealed class AImpl : Either<TA, TB>
        {
            private readonly TA _value;

            public AImpl(TA value)
            {
                _value = value;
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as AImpl);
            }

            public override bool Equals(Either<TA, TB> obj)
            {
                var a = obj as AImpl;
                return a != null
                    && EqualityComparer<TA>.Default.Equals(_value, a._value);
            }

            public override TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b)
            {
                if (a == null) throw new ArgumentNullException("a");
                if (b == null) throw new ArgumentNullException("b");
                return a(_value);
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }

        private sealed class BImpl : Either<TA, TB>
        {
            private readonly TB _value;

            public BImpl(TB value)
            {
                _value = value;
            }

            public override int GetHashCode()
            {
                return _value.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as BImpl);
            }

            public override bool Equals(Either<TA, TB> obj)
            {
                var b = obj as BImpl;
                return b != null
                    && EqualityComparer<TB>.Default.Equals(_value, b._value);
            }

            public override TResult Fold<TResult>(Func<TA, TResult> a, Func<TB, TResult> b)
            {
                if (a == null) throw new ArgumentNullException("a");
                if (b == null) throw new ArgumentNullException("b");
                return b(_value);
            }

            public override string ToString()
            {
                return _value.ToString();
            }
        }
    }
}
