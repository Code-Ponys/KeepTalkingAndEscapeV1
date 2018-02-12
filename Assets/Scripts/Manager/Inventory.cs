        [SerializeField] private GameObject[] _line1 = new GameObject[5];
        [SerializeField] private GameObject[] _line2 = new GameObject[5];
        [SerializeField] private GameObject[] _line3 = new GameObject[5];
        [SerializeField] private GameObject[] _line4 = new GameObject[5];
        [SerializeField] private Image _emptyItemSlot;
        [SerializeField] private Image _ghostSelectorOutline;
        [SerializeField] private Image _humanSelectorOutline;
        private void CreateSlotsArray() {
            for(int j = 0; j < _line1.Length-1; j++) {
                _slots[0, j] = _line1[j];
                _slots[1, j] = _line2[j];
                _slots[2, j] = _line3[j];
                _slots[3, j] = _line4[j];
            }
        }
