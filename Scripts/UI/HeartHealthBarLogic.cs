namespace projectgodot
{
    public enum HeartState
    {
        Empty,  // 빈 하트
        Half,   // 반 찬 하트  
        Full    // 가득 찬 하트
    }

    public class HeartHealthBarLogic
    {
        public const int TOTAL_HEARTS = 3;
        
        public HeartState[] CalculateHeartStates(int currentHealth, int maxHealth)
        {
            if (maxHealth <= 0) return new HeartState[] { HeartState.Empty, HeartState.Empty, HeartState.Empty };
            if (currentHealth < 0) currentHealth = 0;
            
            var hearts = new HeartState[TOTAL_HEARTS];
            
            // 전체 체력을 6단계로 분할 (하트 3개 × 2단계씩)
            // 각 단계는 maxHealth / 6
            float healthPerHalfHeart = (float)maxHealth / (TOTAL_HEARTS * 2);
            
            // 현재 체력에 해당하는 하트 개수 계산 (0.5 단위)
            float totalHeartValue = currentHealth / healthPerHalfHeart;
            
            // 오른쪽부터 순차적으로 하트 상태 결정
            for (int i = 0; i < TOTAL_HEARTS; i++)
            {
                // 현재 하트의 최소 필요 값 (왼쪽부터 0, 1, 2번째 하트)
                float heartMinValue = i * 2; // 각 하트는 2단계(Full=2, Half=1, Empty=0)
                
                if (totalHeartValue >= heartMinValue + 2)
                {
                    // 이 하트를 완전히 채울 수 있음
                    hearts[i] = HeartState.Full;
                }
                else if (totalHeartValue >= heartMinValue + 1)
                {
                    // 이 하트를 반만 채울 수 있음
                    hearts[i] = HeartState.Half;
                }
                else
                {
                    // 이 하트를 채울 수 없음
                    hearts[i] = HeartState.Empty;
                }
            }
            
            return hearts;
        }
        
        public string GetHeartStateText(HeartState state)
        {
            return state switch
            {
                HeartState.Full => "Full Heart",
                HeartState.Half => "Half Heart", 
                HeartState.Empty => "Empty Heart",
                _ => "Unknown"
            };
        }
    }
}