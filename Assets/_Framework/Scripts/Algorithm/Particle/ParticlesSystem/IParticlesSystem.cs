using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public interface IParticlesSystem
    {
        /// <summary>
        /// 设置粒子发射器
        /// </summary>
        /// <param name="emitter"></param>
        void SetParticleEmitter(IParticleEmitter emitter);

        /// <summary>
        /// 设置粒子生成器
        /// </summary>
        /// <param name="generator"></param>
        void SetParticleGenerator(IParticleGenerator generator);

        /// <summary>
        /// 添加粒子更新器
        /// </summary>
        /// <param name="updater"></param>
        void AddParticleUpdater(IParticleUpdater updater);

        /// <summary>
        /// 移除粒子更新器
        /// </summary>
        /// <param name="updater"></param>
        void RemoveParticleUpdater(IParticleUpdater updater);

        /// <summary>
        /// 添加粒子清除器
        /// </summary>
        /// <param name="cleaner"></param>
        void AddParticleCleaner(IParticleCleaner cleaner);

        /// <summary>
        /// 移除粒子清除器
        /// </summary>
        /// <param name="cleaner"></param>
        void RemoveParticleCleaner(IParticleCleaner cleaner);

        /// <summary>
        /// 添加粒子
        /// </summary>
        /// <param name="particle"></param>
        void AddParticle(Particle particle);

        /// <summary>
        /// 移除粒子
        /// </summary>
        /// <param name="particle"></param>
        void RemoveParticle(Particle particle);

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="position"></param>
        void SetPosition(Vector3 position);

        /// <summary>
        /// 是否启动
        /// </summary>
        bool isStart { get; }

        /// <summary>
        /// 是否存活
        /// </summary>
        bool isAlive { get; }

        /// <summary>
        /// 开始
        /// </summary>
        void Start();

        /// <summary>
        /// 停止
        /// </summary>
        void Stop();

        /// <summary>
        /// 重置
        /// </summary>
        void Reset();

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        void Update(float deltaTime);

        /// <summary>
        /// 存活粒子数
        /// </summary>
        int aliveParticleCount { get; }
    }
}
