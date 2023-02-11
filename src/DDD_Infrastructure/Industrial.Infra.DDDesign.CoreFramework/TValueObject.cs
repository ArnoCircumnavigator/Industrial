namespace Industrial.Infra.DDDesign.CoreFramework
{
    public abstract class TValueObject<T> : IValueObject
    {
        public abstract IEnumerable<object> GetAtomicValues();
        public static bool operator ==(TValueObject<T> left, TValueObject<T> right)
        {
            return IsEqual(left, right);
        }
        public static bool operator !=(TValueObject<T> left, TValueObject<T> right)
        {
            return !IsEqual(left, right);
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
            {
                return false;
            }
            TValueObject<T> other = (TValueObject<T>)obj;
            //根据DDD思想，值对象的判等，是取决于内容（值）
            IEnumerator<object> thisValus = this.GetAtomicValues().GetEnumerator();
            IEnumerator<object> otherValus = other.GetAtomicValues().GetEnumerator();
            while (thisValus.MoveNext() && otherValus.MoveNext())
            {
                if (thisValus.Current is null ^ otherValus.Current is null)//null性质不同
                {
                    return false;
                }
                if (thisValus.Current != null && !thisValus.Current.Equals(otherValus.Current))
                {
                    return false;
                }
            }
            return !(thisValus.MoveNext() && otherValus.MoveNext());//都移动到最后了
        }
        public override int GetHashCode()
        {
            return GetAtomicValues()
                .Select(x => x != null ? x.GetHashCode() : 0)
                .Aggregate((x, y) => x ^ y);
        }
        private static bool IsEqual(TValueObject<T> left, TValueObject<T> right)
        {
            if (left is null ^ right is null)
            {
                return false;
            }
            return left is null || left.Equals(right);
        }
    }
}
