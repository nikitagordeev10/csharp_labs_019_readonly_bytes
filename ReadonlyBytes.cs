/* Условие
using System;
using System.Collections;
using System.Collections.Generic;

namespace hashes {
    // TODO: Создайте класс ReadonlyBytes
}
*/

/* Решение 1
//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace hashes {
//    public class ReadonlyBytes : IEnumerable<byte> {
//        private readonly byte[] bytes; // закрытое поле массив байтов

//        public ReadonlyBytes(params byte[] bytes) { // конструктор, принимает байты, создает экземпляр массива
//            if (bytes == null) { // проверка, что массив не null
//                throw new ArgumentNullException(nameof(bytes)); // иначе исключение
//            }
//            this.bytes = bytes; // инициализируем поле bytes
//        }

//        public int Length { // свойство возвращает длину массива байтов
//            get { return bytes.Length; }
//        }

//        public byte this[int index] { // индексатор возвращает байт с указанным индексом массива
//            get { return bytes[index]; }
//        }

//        public override bool Equals(object obj) { // проверяет объект на равенство текущему объекту
//            if (obj == null || GetType() != obj.GetType()) { // объект null или не является экземпляром класса ReadonlyBytes
//                return false; 
//            }

//            var other = (ReadonlyBytes)obj; // приведение obj к типу ReadonlyBytes 
//            if (bytes.Length != other.bytes.Length) { // длины массивов не совпадают
//                return false; 
//            }

//            if (other.bytes == null) {
//                throw new NullReferenceException(nameof(other.bytes)); // Проверка, что other.bytes != null
//            }

//            for (int i = 0; i < bytes.Length; i++) { // сравниваем элементы массивов
//                if (bytes[i] != other.bytes[i]) { // хотя бы один не совпадает
//                    return false;
//                }
//            }

//            return true; // массивы полностью совпадают
//        }

//        public override int GetHashCode() { // метод вычисляет хэш-код для объекта используя алгоритм FNV-1 из Википедии
//            var hash = 0; // начальное значение хэша
//            const int prime = 10007; // простое число для вычисления хэш-кода

//            unchecked { // отключить проверку на переполнение
//                foreach (var b in bytes) {
//                    hash = hash * prime + b; // вычисление хэш-кода
//                }
//            }

//            return hash;
//        }

//        public override string ToString() { // метод возвращает строковое представление объекта
//            string result = "[";

//            for (int i = 0; i < Length; i++) { // проходим по элементам массива
//                result += bytes[i].ToString(); // добавляем элемент в строку

//                if ((i + 1) != Length) // элемент не последний
//                    result += ", "; // добавляем запятую и пробел
//            }

//            result += "]"; // закрываем строку
//            return result;
//        }

//        public IEnumerator<byte> GetEnumerator() { // перечислитель для bytes
//            foreach (byte b in bytes) {
//                yield return b; // возвращает элемент типа byte
//            }
//            yield break; // завершает последовательность элементов
//        }

//        IEnumerator IEnumerable.GetEnumerator() { // интерфейс возвращает enumerator
//            return GetEnumerator();
//        }
//    }
//}
*/

/* Решение 2
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes {
    public class ReadonlyBytes : IEnumerable<byte> {
        private readonly byte[] bytes; // закрытое поле массив байтов

        public ReadonlyBytes(params byte[] bytes) { // конструктор, принимает байты, создает экземпляр массива
            if (bytes == null) { // Проверка, что массив не null
                throw new ArgumentNullException(nameof(bytes)); // иначе исключение
            }
            this.bytes = bytes; // Инициализируем поле bytes
        }

        public int Length { // свойство возвращает длину массива байтов
            get { return bytes.Length; }
        }

        public byte this[int index] { // индексатор возвращает байт с указанным индексом массива
            get { return bytes[index]; }
        }

        public override bool Equals(object obj) { // метод возвращает true, если объекты равны по содержанию
            if (obj == null) {  // проверка, что объект не равен null
                return false;
            }
            if (object.ReferenceEquals(this, obj)) { // проверяем, что объекты ссылаются на один и тот же экземпляр класса
                return true;
            }

            bool areTypesEqual = obj.GetType() == GetType(); // сравниваем тип объекта
            bool areObjectsEqual = Equals((ReadonlyBytes)obj); // вызываем метод Equals класса ReadonlyBytes
            return areTypesEqual && areObjectsEqual;
        }

        private bool Equals(ReadonlyBytes other) { // метод сравнивает два экземпляра класса ReadonlyBytes на содержание
            if (Length != other.Length) return false; // длины массивов равны

            for (var i = 0; i < Length; i++) // сравниваем элементы массивов
                if (bytes[i] != other.bytes[i])
                    return false;

            return true;
        }

        public override int GetHashCode() { // метод вычисляет хэш-код для объекта используя алгоритм FNV-1 из Википедии
            const uint fnvOffset = 2166136261u; // начальное значение хэша
            const uint fnvPrime = 16777619u; // простое число, используется в вычислениях
            uint hash = fnvOffset; // инициализируем переменную hash значением fnvOffset

            unchecked { // отключить проверку на переполнение
                foreach (byte b in bytes) { // Вычисляем хэш
                    hash ^= b; // исключающее ИЛИ
                    hash *= fnvPrime; // умножаение на константу
                }

                return (int)hash;
            }
        }

        public override string ToString() { // метод возвращает строковое представление объекта
            var result = new StringBuilder("["); // экземпляр для формирования строки
            
            for (int i = 0; i < Length; i++) { // проходим по элементам массива
                result.Append(bytes[i]); // Добавляем элемент в строку

                if ((i + 1) != Length) // элемент не последний, добавляем запятую и пробел
                    result.Append(", "); // добавляем запятую и пробел
            }

            result.Append("]"); // Закрываем строку
            return result.ToString();
        }

        public IEnumerator<byte> GetEnumerator() { // перечислитель для bytes
            foreach (byte b in bytes) {
                yield return b; // Возвращает очередной элемент типа byte по мере обхода коллекции
            }
            yield break; // Завершает последовательность элементов
        }

        IEnumerator IEnumerable.GetEnumerator() { // интерфейс возвращает enumerator
            return GetEnumerator();
        }
    }
}
*/

/* Решение 3
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes {
    public class ReadonlyBytes : IEnumerable<byte> {
        private readonly byte[] bytes; // закрытое поле массив байтов

        public ReadonlyBytes(params byte[] bytes) { // конструктор, принимает байты, создает экземпляр массива
            if (bytes == null) { // проверка, что массив не null
                throw new ArgumentNullException(nameof(bytes)); // иначе исключение
            }
            this.bytes = bytes; // инициализируем поле bytes
        }

        public int Length { // свойство возвращает длину массива байтов
            get { return bytes.Length; }
        }

        public byte this[int index] { // индексатор возвращает байт с указанным индексом массива
            get { return bytes[index]; }
        }

        public override bool Equals(object obj) { // проверяет объект на равенство текущему объекту
            if (obj == null || GetType() != obj.GetType()) { // объект null или не является экземпляром класса ReadonlyBytes
                return false;
            }

            var other = (ReadonlyBytes)obj; // приведение obj к типу ReadonlyBytes 
            if (bytes.Length != other.bytes.Length) { // длины массивов не совпадают
                return false;
            }

            if (other.bytes == null) {
                throw new NullReferenceException(nameof(other.bytes)); // Проверка, что other.bytes != null
            }

            for (int i = 0; i < bytes.Length; i++) { // сравниваем элементы массивов
                if (bytes[i] != other.bytes[i]) { // хотя бы один не совпадает
                    return false;
                }
            }

            return true; // массивы полностью совпадают
        }

        private const int PRIME = 31; // статическое поле, чтобы избежать вычисления значения при каждом вызове метода GetHashCode.
        private static readonly int HashSeed = Guid.NewGuid().GetHashCode(); // уникальное начальное значение хэша
        public override int GetHashCode() { // метод вычисляет хэш-код для объекта используя алгоритм FNV-1 из Википедии
            var hash = HashSeed; // начальное значение хэша

            if (bytes == null) { 
                return hash;
            }

            foreach (var b in bytes) { 
                unchecked { // отключить проверку на переполнение
                    hash = hash * PRIME + b; // вычисление хэш кода
                }
            }

            return hash;
        }

        public override string ToString() { // метод возвращает строковое представление объекта.
            if (Length == 0) { 
                return "[]"; // метод возвращает пустой массив строк
            }

            var result = new StringBuilder("["); // объект StringBuilder
            result.Append(bytes[0].ToString()); // добавляет элементы массива bytes

            for (int i = 1; i < Length; i++) {
                result.Append(", "); // разделяя их запятыми и пробелами
                result.Append(bytes[i].ToString()); // добавляет элементы массива bytes
            }

            result.Append("]"); // закрывающая скобка.
            return result.ToString();
        }

        public IEnumerator<byte> GetEnumerator() { // перечислитель для bytes
            foreach (var b in bytes) {
                yield return b; // возвращает элемент типа byte
            }
        }

        IEnumerator IEnumerable.GetEnumerator() { // интерфейс возвращает enumerator
            return GetEnumerator();
        }
    }
}
*/

/*  Решение 4 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace hashes {
    public class ReadonlyBytes : IEnumerable<byte> {
        private readonly byte[] bytes; // закрытое поле массив байтов
        private readonly int hashCode; // значение хэша для текущих байтов

        public ReadonlyBytes(params byte[] bytes) { // конструктор, принимает байты, создает экземпляр массива
            if (bytes == null) { // проверка, что массив не null
                throw new ArgumentNullException(nameof(bytes)); // иначе исключение
            }
            this.bytes = bytes; // инициализируем поле bytes
            this.hashCode = CalculateHashCode(bytes);
        }

        public int Length { // свойство возвращает длину массива байтов
            get { return bytes.Length; }
        }

        public byte this[int index] { // индексатор возвращает байт с указанным индексом массива
            get { return bytes[index]; }
        }

        public override bool Equals(object obj) { // проверяет объект на равенство текущему объекту
            if (obj == null || GetType() != obj.GetType()) { // объект null или не является экземпляром класса ReadonlyBytes
                return false;
            }

            var other = (ReadonlyBytes)obj; // приведение obj к типу ReadonlyBytes 
            if (bytes.Length != other.bytes.Length) { // длины массивов не совпадают
                return false;
            }

            if (other.bytes == null) {
                throw new NullReferenceException(nameof(other.bytes)); // Проверка, что other.bytes != null
            }

            for (int i = 0; i < bytes.Length; i++) { // сравниваем элементы массивов
                if (bytes[i] != other.bytes[i]) { // хотя бы один не совпадает
                    return false;
                }
            }

            return true; // массивы полностью совпадают
        }

        private static int CalculateHashCode(byte[] bytes) { // вычисляет хэш-код для массива байтов
            int hash = new Guid().GetHashCode(); // уникальное начальное значение хэша

            foreach (var b in bytes) {
                unchecked { // отключение проверки на переполнение
                    hash = hash * 31 + b; // вычисление хэш кода
                }
            }

            return hash;
        }

        public override int GetHashCode() { // метод возвращает значение хэша для текущих байтов
            return hashCode;
        }

        public override string ToString() { // метод возвращает строковое представление объекта.
            if (Length == 0) {
                return "[]"; // метод возвращает пустой массив строк
            }

            var result = new StringBuilder("["); // объект StringBuilder
            result.Append(bytes[0].ToString()); // добавляет элементы массива bytes

            for (int i = 1; i < Length; i++) {
                result.Append(", "); // разделяя их запятыми и пробелами
                result.Append(bytes[i].ToString()); // добавляет элементы массива bytes
            }

            result.Append("]"); // закрывающая скобка.
            return result.ToString();
        }

        public IEnumerator<byte> GetEnumerator() { // перечислитель для bytes
            foreach (var b in bytes) {
                yield return b; // возвращает элемент типа byte
            }
        }

        IEnumerator IEnumerable.GetEnumerator() { // интерфейс возвращает enumerator
            return GetEnumerator();
        }
    }
}