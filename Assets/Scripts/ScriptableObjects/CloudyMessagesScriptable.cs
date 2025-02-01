using UnityEngine;
using System.Collections.Generic;

namespace ScriptableObjects
{
    [CreateAssetMenu]
    public class CloudyMessagesScriptable : ScriptableObject
    {
        [SerializeField] private List<string> messages = new List<string>
        {
            "President's Endorsement!",
            "Billionaire's Investment!",
            "Tech Giant Partnership!",
            "Wall Street Backing!",
            "Celebrity Endorsement!",
            "Major Exchange Listing!",
            "Institutional Adoption!",
            "Viral Trading Trend!",
            "Fortune 500 Integration!",
            "Global Bank Support!",
            "Hedge Fund Investment!",
            "Social Media Frenzy!",
            "Innovation Award!",
            "Market Breakthrough!",
            "Strategic Merger!",
            "Crypto Influencer Boost!",
            "Revolutionary Update!",
            "Government Approval!",
            "Industry Recognition!",
            "Venture Capital Funding!",
            "Trading Volume Spike!",
            "Corporate Adoption!",
            "Market Leadership!",
            "International Partnership!",
            "Security Upgrade!",
            "Mainstream Adoption!",
            "Expert Recommendation!",
            "Trading Bot Integration!",
            "Blockchain Innovation!",
            "Community Growth!",
            "Development Milestone!",
            "Market Expansion!",
            "Strategic Alliance!",
            "Trading Platform Launch!",
            "Regulatory Compliance!",
            "Investment Fund Entry!",
            "Technical Breakthrough!",
            "Global Recognition!",
            "Mass Media Coverage!",
            "Market Dominance!"
        };

        public List<string> Messages => messages;
    }
}
