using System.Collections.Generic;
using UnityEngine;

namespace WaifuDriver
{
    public class StarsGUI : MonoBehaviour
    {
        [SerializeField] private Transform _starTemplate;

        private List<Transform> _stars = new List<Transform>();

        public void Start()
        {
            this._starTemplate.gameObject.SetActive(false);
        }

        private void _EnsureAllStarsAreInstanced(int count)
        {
            var delta = count - this._stars.Count;
            for (int i = 0; i < delta; i++) {
                var star = Object.Instantiate(this._starTemplate);
                star.gameObject.SetActive(true);
                star.transform.SetParent(this.transform);
                this._stars.Add(star);
            }
        }

        public void SetStarsCount(int count)
        {
            this._EnsureAllStarsAreInstanced(count);

            for (int i = 0; i < this._stars.Count; i++) {
                this._stars[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < count; i++) {
                this._stars[i].gameObject.SetActive(true);
            }
        }
    }
}