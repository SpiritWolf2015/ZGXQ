using UnityEngine;
using System.Collections.Generic;
using System.Threading;

/// <summary>
/// 工作线程管理类
/// </summary>
public class ThreadManager  {
    
    private static List<Thread> s_workThreads = new List<Thread>( );
    

    #region Lazy模式线程安全单例

    private static ThreadManager s_instance = null;
    private static object s_lockObj = new object( );

    /// <summary>
    /// 使用了双重锁方式较好地解决了多线程下的单例模式实现。
    /// 先看内层的if语句块，使用这个语句块时，先进行加锁操作，
    /// 保证只有一个线程可以访问该语句块，进而保证只创建了一个实例。
    /// 再看外层的if语句块，这使得每个线程欲获取实例时不必每次都得加锁，
    /// 因为只有实例为空时（即需要创建一个实例），才需加锁创建，
    /// 若果已存在一个实例，就直接返回该实例，节省了性能开销。
    /// </summary>
    public static ThreadManager Instance {
        get {
            if ( null == s_instance ) {
                lock ( s_lockObj ) {
                    if ( null == s_instance ) {
                        s_instance = new ThreadManager( );
                    }                    
                }
            }
            return s_instance;
        }
    }

    #endregion Lazy模式线程安全单例

    /// <summary>
    /// 加入线程管理，并启动子线程为就绪状态。
    /// </summary>
    /// <param name="workThread"></param>
    public void addWorkThread ( Thread workThread ) {
        lock ( this ) {
            s_workThreads.Add(workThread);
            workThread.Start( );
        }
    }

    /// <summary>
    /// 从线程管理移除子线程，并停止子线程。
    /// </summary>
    /// <param name="workThread"></param>
    public void removeWorkThread ( Thread workThread ) {
        lock ( this ) {
            killWorkThread(workThread);
            s_workThreads.Remove(workThread);
        }
    }

    /// <summary>
    /// 停止所有子线程
    /// </summary>
    public void removeAllWorkThreads (  ) {
        lock ( this ) {
            foreach ( Thread workThread in s_workThreads ) {                
                killWorkThread(workThread);
            }
            s_workThreads.Clear( );
        }
    }

    private void killWorkThread ( Thread workThread ) {
        workThread.Abort( );
        //workThread.Join(5);
        Debuger.LogWarning(string.Format("结束线程！Thread ID={0}", workThread.ManagedThreadId));
    }

}
