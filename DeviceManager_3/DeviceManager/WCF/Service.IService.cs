using System;
using System.Xml;
using System.Threading;
using System.ServiceModel;
using System.Collections.Generic;

using DeviceManager;
using DeviceManager.DevMan;

namespace WCF
{
    public partial class Service : IService
    {
        // ---- команды управления пользователем ----

        /// <summary>
        /// Зарегистрироваться в системе
        /// </summary>
        /// <param name="Role">Требуемая роль в системе</param>
        /// <returns>Описатель используемый при взаимодействии в системе в дальнейшем</returns>
        public Handle Register(Role Role)
        {
            try
            {
                locker.AcquireWriterLock(300);
                try
                {
                    /*if (Role == Role.Basic)
                    {
                        foreach (var user in users)
                        {
                            if (user.Role == Role.Basic)
                            {
                                return null;
                            }
                        }
                    }*/

                    ICallBack proxy = OperationContext.Current.GetCallbackChannel<ICallBack>();
                    if (proxy != null)
                    {
                        User user = User.InstanceUser(proxy, Role);
                        users.Add(user);

                        return user.Handle;
                    }
                }
                finally
                {
                    locker.ReleaseWriterLock();
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// Установить режим работы
        /// </summary>
        /// <param name="mode">Требуемый режим работы</param>
        /// <param name="handle">Описатель пользователя для которого установливается режим работы</param>
        public void SetMode(Handle handle, UserMode mode)
        {
            try
            {
                locker.AcquireWriterLock(500);
                try
                {
                    foreach (User user in users)
                    {
                        if (user.Handle.Identifier == handle.Identifier)
                        {
                            user.Mode = mode;
                            break;
                        }
                    }
                }
                finally
                {
                    locker.ReleaseWriterLock();
                }

            }
            catch { }
            return;
        }

        /// <summary>
        /// Получить текущий режим работы пользователя
        /// </summary>
        /// <param name="handle">Описатель пользователя</param>
        /// <returns>Текущий режим работы пользователя</returns>
        public UserMode GetCurrentMode(Handle handle)
        {
            try
            {
                locker.AcquireReaderLock(300);
                try
                {
                    foreach (User user in users)
                    {
                        if (user.Handle.Identifier == handle.Identifier)
                        {
                            return user.Mode;
                        }
                    }
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }

            }
            catch { }
            return UserMode.Default;
        }

        // ---- команды управления параметрами ----


        /// <summary>
        /// Установить параметры, которые необходимо передавать
        /// </summary>
        /// <param name="handle">Идентификатор пользователя</param>
        /// <param name="Indexes">Массив индексов параметров</param>
        public void SelectParameters(Handle handle, int[] Indexes)
        {
            try
            {
                locker.AcquireReaderLock(100);
                try
                {
                    foreach (var user in users)
                    {
                        if (user.Handle.Identifier == handle.Identifier)
                        {
                            if (Indexes != null)
                            {
                                user.SetIndexes(Indexes);
                                user.Selected = true;
                            }

                            return;
                        }
                    }
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Очистить список параметров для отправки пользователю
        /// </summary>
        /// <param name="handle">Описатель пользователя</param>
        public void ClearSelectedParameters(Handle handle)
        {
            try
            {
                locker.AcquireReaderLock(100);
                try
                {
                    foreach (var user in users)
                    {
                        if (user.Handle.Identifier == handle.Identifier)
                        {
                            user.ClearIndexes();
                            user.Selected = false;
                        }
                    }
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Получить список параметров с описаниями и номерами в списке
        /// </summary>
        /// <returns>Список имеющихся параметров</returns>
        public PDescription[] GetParametersDescription()
        {
            if (app != null)
            {
                Formula[] formuls = app.Converter.Formuls;
                if (formuls != null)
                {
                    PDescription[] descriptions = new PDescription[formuls.Length];
                    for (int i = 0; i < descriptions.Length; i++)
                    {
                        descriptions[i] = new PDescription(formuls[i].Position, formuls[i].Macros.Description, formuls[i].Type);
                    }

                    return descriptions;
                }
            }
            return null;
        }

        /// <summary>
        /// Присвоить параметру указанное значение
        /// </summary>
        /// <param name="handle">Описатель пользователя</param>
        /// <param name="Index">Номер параметра в списке</param>
        /// <param name="Value">Значение которое присвоить параметру</param>
        public void SetParameterValue(Handle handle, Int32 Index, Single Value)
        {
            try
            {
                locker.AcquireReaderLock(100);
                try
                {
                    foreach (var user in users)
                    {
                        if (user.Handle.Identifier == handle.Identifier)
                        {
                            if (app != null)
                            {
                                app.Converter.SetParameterValue(Index, Value);
                            }
                        }
                    }
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Отправить пакет в порт
        /// </summary>
        /// <param name="packet">Пакет для отправки в COM порт. Версия TCP протокола обмена</param>
        public void ToCOM(string packet)
        {
            try
            {
                Packet pack = new Packet();

                pack.Com_Packet = PacketTranslater.TranslateToUnigueFormatTcpPacket(packet, app.TypeCRC);
                pack.Tcp_Packet = PacketTranslater.FromUnigueToTcp(pack.Com_Packet);

                pack.Wait = false;
                if (app.IProtocol.IsToDevice(pack.Tcp_Packet))
                {
                    if (app.IProtocol.IsRead(pack.Tcp_Packet))
                    {
                        pack.Wait = true;
                    }
                }
                place.Insert(pack);
            }
            catch { }
        }

        /// <summary>
        /// Запросить срез данных
        /// </summary>
        /// <param name="handle">Дескриптор пользователя</param>
        public void RequestDataSlice(Handle handle)
        {
            try
            {
                locker.AcquireReaderLock(100);
                try
                {
                    foreach (var user in users)
                    {
                        if (user.Handle.Identifier == handle.Identifier)
                        {
                            DateTime now = DateTime.Now;
                            Float[] results = app.Converter.GetResults();

                            if (results != null)
                            {
                                Single[] slice = new float[results.Length];
                                for (int i = 0; i < results.Length; i++)
                                {
                                    slice[i] = results[i].GetCurrentValue();
                                }                                                                

                                if (user.Selected) SendToUser(user, now, slice);
                                else
                                    user.Interface.SendAll(now, slice);
                            }
                        }
                    }
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch { }
        }

        /// <summary>
        /// Запросить срез данных
        /// </summary>
        /// <param name="handle">Дескриптор  пользователя</param>
        /// <returns>Срез данных</returns>
        public Single[] GetDataSlice(Handle handle)
        {
            try
            {
                locker.AcquireReaderLock(100);
                try
                {
                    foreach (var user in users)
                    {
                        if (user.Handle.Identifier == handle.Identifier)
                        {
                            DateTime now = DateTime.Now;
                            Float[] results = app.Converter.GetResults();

                            if (results != null)
                            {
                                Single[] slice = new float[results.Length];
                                for (int i = 0; i < results.Length; i++)
                                {
                                    slice[i] = results[i].GetCurrentValue();
                                }

                                if (user.Selected) return FilterSliceForUser(user, now, slice);
                                else
                                    return slice;
                            }
                        }
                    }
                }
                finally
                {
                    locker.ReleaseReaderLock();
                }
            }
            catch { }
            return null;
        }

        /// <summary>
        /// отправляем требуемые параметры пользователю
        /// </summary>
        /// <param name="user">Пользователь которому отправлять параметры</param>
        /// <param name="Time">Время когда был сделан срез данных</param>
        /// <param name="Slice">Срез данных на текущий момент</param>
        private Single[] FilterSliceForUser(User user, DateTime Time, Single[] Slice)
        {
            try
            {
                int[] indexes = user.Indexes;
                if (indexes != null)
                {
                    float[] parameters = new float[indexes.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        int index = indexes[i];
                        parameters[i] = float.NaN;

                        if (index > -1 && index < Slice.Length)
                        {
                            parameters[i] = Slice[index];
                        }
                    }

                    return parameters;
                }
            }
            catch { }
            return null;
        }


        // ---- статические методы ----

        /// <summary>
        /// Передать пользователю данные
        /// </summary>
        /// <param name="Time">Время получения среза данных</param>
        /// <param name="Slice">Срез данных</param>
        public static void SendData(DateTime Time, Single[] Slice)
        {
            try
            {
                locker.AcquireReaderLock(100);
                try
                {
                    foreach (var user in users)
                    {
                        switch (user.Mode)
                        {
                            case UserMode.Active:

                                break;

                            case UserMode.Passive:

                                if (user.Selected)
                                {
                                    // ---- отправляем запрошенные пользователем параметры ----

                                    SendToUser(user, Time, Slice);
                                }
                                else
                                {
                                    // ---- отправляем все параметры пользователю ----

                                    try
                                    {
                                        user.Interface.SendAll(Time, Slice);
                                    }
                                    catch 
                                    {
                                        user.Fail = true;
                                    }
                                }

                                break;

                            case UserMode.Default:

                                break;

                            default:

                                break;
                        }
                    }

                    users.RemoveAll(FailUser);
                }
                finally
                {   
                    locker.ReleaseReaderLock();
                }
            }
            catch { }
        }

        private static bool FailUser(User user)
        {
            if (user.Fail) return true;
            else
                return false;
        }

        /// <summary>
        /// отправляем требуемые параметры пользователю
        /// </summary>
        /// <param name="user">Пользователь которому отправлять параметры</param>
        /// <param name="Time">Время когда был сделан срез данных</param>
        /// <param name="Slice">Срез данных на текущий момент</param>
        private static void SendToUser(User user, DateTime Time, Single[] Slice)
        {
            try
            {
                int[] indexes = user.Indexes;
                if (indexes != null)
                {
                    float[] parameters = new float[indexes.Length];
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        int index = indexes[i];
                        if (index > -1 && index < Slice.Length)
                        {
                            parameters[i] = Slice[index];
                        }
                    }

                    try
                    {
                        user.Interface.SendAll(Time, parameters);
                    }
                    catch
                    {
                        user.Fail = true;
                    }
                }
            }
            catch
            {
                //user.Fail = true;
            }
        }
    }
}