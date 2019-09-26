using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Framework
{
    public class ParticlesSystem : IParticlesSystem
    {
        protected IParticleEmitter _emitter;
        protected IParticleGenerator _generator;
        protected List<IParticleUpdater> _updaters;
        protected List<IParticleCleaner> _cleaners;
        protected List<Particle> _particles;
        protected bool _isStart;
        protected int _aliveParticleCount;

        public ParticlesSystem()
        {
            _updaters = new List<IParticleUpdater>(1);
            _cleaners = new List<IParticleCleaner>(1);
            _particles = new List<Particle>(1);
        }

        /// <summary>
        /// 设置粒子发射器
        /// </summary>
        /// <param name="emitter"></param>
        public virtual void SetParticleEmitter(IParticleEmitter emitter)
        {
            _emitter = emitter;
            if (_generator != null)
                _emitter.AttachGenerator(_generator);
        }

        /// <summary>
        /// 设置粒子生成器
        /// </summary>
        /// <param name="generator"></param>
        public virtual void SetParticleGenerator(IParticleGenerator generator)
        {
            _generator = generator;
            if (_generator != null)
                _emitter.AttachGenerator(_generator);
        }

        /// <summary>
        /// 添加粒子更新器
        /// </summary>
        /// <param name="updater"></param>
        public virtual void AddParticleUpdater(IParticleUpdater updater)
        {
            _updaters.Add(updater);
        }

        /// <summary>
        /// 移除粒子更新器
        /// </summary>
        /// <param name="updater"></param>
        public virtual void RemoveParticleUpdater(IParticleUpdater updater)
        {
            _updaters.Remove(updater);
        }

        /// <summary>
        /// 添加粒子清除器
        /// </summary>
        /// <param name="cleaner"></param>
        public virtual void AddParticleCleaner(IParticleCleaner cleaner)
        {
            _cleaners.Add(cleaner);
        }
        /// <summary>
        /// 移除粒子清除器
        /// </summary>
        /// <param name="cleaner"></param>
        public virtual void RemoveParticleCleaner(IParticleCleaner cleaner)
        {
            _cleaners.Remove(cleaner);
        }

        /// <summary>
        /// 添加粒子
        /// </summary>
        /// <param name="particle"></param>
        public virtual void AddParticle(Particle particle)
        {
            _particles.Add(particle);
        }

        /// <summary>
        /// 移除粒子
        /// </summary>
        /// <param name="particle"></param>
        public virtual void RemoveParticle(Particle particle)
        {
            _particles.Remove(particle);
        }

        /// <summary>
        /// 设置位置
        /// </summary>
        /// <param name="position"></param>
        public virtual void SetPosition(Vector3 position)
        {
            if (_emitter != null)
                _emitter.SetPosition(position);
        }

        /// <summary>
        /// 是否启动
        /// </summary>
        public virtual bool isStart
        {
            get { return _isStart; }
        }

        /// <summary>
        /// 是否存活
        /// </summary>
        public virtual bool isAlive
        {
            get { return _isStart; }
        }

        /// <summary>
        /// 开始
        /// </summary>
        public virtual void Start()
        {
            _isStart = true;
        }

        /// <summary>
        /// 停止
        /// </summary>
        public virtual void Stop()
        {
            _isStart = false;
        }

        /// <summary>
        /// 重置
        /// </summary>
        public virtual void Reset()
        {
            if (_emitter != null)
                _emitter.Reset();
            if (_generator != null)
                _generator.Reset();
            foreach (IParticleUpdater updater in _updaters)
                updater.Reset();
            foreach (IParticleCleaner cleaner in _cleaners)
                cleaner.Reset();

            _particles.Clear();
            _isStart = false;
            _aliveParticleCount = 0;
        }

        /// <summary>
        /// 存活粒子数
        /// </summary>
        public virtual int aliveParticleCount
        {
            get { return _aliveParticleCount; }
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="deltaTime"></param>
        public virtual void Update(float deltaTime)
        {
            if (_isStart == false)
                return;

            foreach (Particle particle in _particles)
            {
                if (particle.isAlive == true)
                {
                    foreach (IParticleUpdater updater in _updaters)
                    {
                        updater.UpdateParticle(particle, deltaTime);
                    }
                }
            }

            _aliveParticleCount = 0;

            foreach (Particle particle in _particles)
            {
                if (particle.isAlive == true)
                {
                    foreach (IParticleCleaner cleaner in _cleaners)
                    {
                        if (cleaner.CleanParticle(particle) == true)
                        {
                            ProcessDeadParticle(particle);
                            break;
                        }
                    }
                }

                if (particle.isAlive == true)
                    _aliveParticleCount++;
            }

            if (_aliveParticleCount == 0)
                Stop();
        }

        /// <summary>
        /// 处理死亡粒子
        /// </summary>
        /// <param name="particle"></param>
        protected virtual void ProcessDeadParticle(Particle particle)
        {

        }
    }
}
