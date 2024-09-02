using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

namespace Client
{
    public class PresentationPageView : BaseWindowView
    {
        [SerializeField] private Transform content;
        [SerializeField] private SubjectItem subjectItemPrefab;

        private List<SubjectItem> _subjectItems = new();
        
        protected override void OnEnable()
        {
            base.OnEnable();
            //SpawnItems();
        }

        private void SpawnItems()
        {
            if (_subjectItems.Count >= 10)
                return;
            
            for (int i = 0; i < 10; i++)
            {
                var item = Instantiate(subjectItemPrefab, content);
                item.subjectPopUpView = subjectPopUpView;
                _subjectItems.Add(item);
            }
        }
    }
}