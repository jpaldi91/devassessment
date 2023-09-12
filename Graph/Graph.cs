using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace Graph
{
    public interface IGraph<T>
    {
        IObservable<IEnumerable<T>> RoutesBetween(T source, T target);
    }

    public class Graph<T> : IGraph<T>
    {
        private readonly Dictionary<T, IEnumerable<T>> _dict;

        public Graph(IEnumerable<ILink<T>> links)
        {
            _dict = links.GroupBy(l => l.Source)
                         .ToDictionary(g => g.Key, g => g.Select(l => l.Target));
        }

        public IObservable<IEnumerable<T>> RoutesBetween(T source, T target)
        {
            List<IEnumerable<T>> allPaths = new List<IEnumerable<T>>();
            Stack<T> stack = new Stack<T>();
            HashSet<T> visitedNodes = new HashSet<T>();

            DFS(source, target, stack, visitedNodes, allPaths);

            return allPaths.ToObservable();
        }

        private void DFS(T currentNode, T targetNode, Stack<T> stack, HashSet<T> visitedNodes, List<IEnumerable<T>> allPaths)
        {
            visitedNodes.Add(currentNode);
            stack.Push(currentNode);

            if (currentNode.Equals(targetNode))
            {
                // valid path, add the stack as a list in the valid paths
                allPaths.Add(new List<T>(stack.Reverse()));
            }
            else
            {
                foreach (T neighbor in _dict.GetValueOrDefault(currentNode))
                {
                    if (!visitedNodes.Contains(neighbor))
                    {
                        DFS(neighbor, targetNode, stack, visitedNodes, allPaths);
                    }
                }
            }

            // backtracking, this is necessary in cases where we want all possible paths to the target
            visitedNodes.Remove(currentNode);
            stack.Pop();
        }
    }
}
