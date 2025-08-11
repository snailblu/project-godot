using Godot;

namespace projectgodot
{
    /// <summary>
    /// Events AutoLoad 노드에 안전하게 접근하기 위한 헬퍼 클래스
    /// 모든 Events 노드 접근을 중앙화하여 코드 중복을 제거합니다.
    /// </summary>
    public static class EventsHelper
    {
        /// <summary>
        /// Events AutoLoad 노드를 안전하게 가져옵니다.
        /// 테스트 환경에서는 null을 반환하여 안전하게 처리됩니다.
        /// </summary>
        /// <param name="node">GetNode를 호출할 수 있는 노드 (일반적으로 this)</param>
        /// <returns>Events 노드 인스턴스 (테스트 환경에서는 null)</returns>
        public static Events GetEventsNode(Node node)
        {
            if (EnvironmentHelper.IsTestEnvironment())
            {
                return null;
            }

            return node?.GetNode<Events>("/root/Events");
        }

        /// <summary>
        /// Events AutoLoad 노드를 안전하게 가져옵니다. (Null-safe 버전)
        /// 노드를 찾을 수 없는 경우에도 예외를 발생시키지 않습니다.
        /// </summary>
        /// <param name="node">GetNodeOrNull을 호출할 수 있는 노드 (일반적으로 this)</param>
        /// <returns>Events 노드 인스턴스 (찾을 수 없거나 테스트 환경에서는 null)</returns>
        public static Events GetEventsNodeSafe(Node node)
        {
            if (EnvironmentHelper.IsTestEnvironment())
            {
                return null;
            }

            return node?.GetNodeOrNull<Events>("/root/Events");
        }

        /// <summary>
        /// Events 노드가 사용 가능한지 확인합니다.
        /// </summary>
        /// <param name="node">확인할 노드 인스턴스</param>
        /// <returns>Events 노드가 사용 가능하면 true, 아니면 false</returns>
        public static bool IsEventsAvailable(Node node)
        {
            return GetEventsNode(node) != null;
        }

        /// <summary>
        /// Events 노드에 신호를 안전하게 발생시킵니다.
        /// 테스트 환경이나 Events 노드가 없는 경우 무시됩니다.
        /// </summary>
        /// <param name="node">신호를 발생시킬 노드</param>
        /// <param name="signal">발생시킬 신호</param>
        /// <param name="args">신호 인수</param>
        public static void EmitSignalSafe(Node node, StringName signal, params Variant[] args)
        {
            var events = GetEventsNode(node);
            events?.EmitSignal(signal, args);
        }
    }
}