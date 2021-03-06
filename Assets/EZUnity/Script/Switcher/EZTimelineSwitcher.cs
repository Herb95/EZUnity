/* Author:          ezhex1991@outlook.com
 * CreateTime:      2018-05-18 10:06:26
 * Organization:    #ORGANIZATION#
 * Description:     
 */
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace EZUnity
{
    public class EZTimelineSwitcher : EZSwitcher<TimelineAsset>
    {
        [SerializeField]
        private PlayableDirector m_Director;
        public PlayableDirector director { get { return m_Director; } }

        public override void Switch(int index)
        {
            director.playableAsset = options[index];
        }
    }
}
