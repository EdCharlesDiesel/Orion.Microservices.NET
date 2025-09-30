// using Orion.DataAccess.Postgres.Entities.Common;
// using Orion.Domain.IRepositories;
//
// namespace Orion.DataAccess.Postgres.AllFeatures
// {
//     public class FeatureManager : IFeatureManager
//     {
//         private readonly IUsernameProvider _usernameProvider;
//         private readonly Dictionary<string, bool> _featureConfigurations;
//
//         public FeatureManager(
//             IFeatureRepository repository, 
//             IUsernameProvider usernameProvider, 
//             Dictionary<string, bool> featureConfigurations)
//         {
//             _usernameProvider = usernameProvider ?? throw new ArgumentNullException(nameof(usernameProvider));
//             if (repository == null) throw new ArgumentNullException(nameof(repository));
//
//             _featureConfigurations = featureConfigurations ?? new Dictionary<string, bool>();
//
//             Initialize(repository);
//         }
//
//         private FeatureManager(
//             IList<Feature> features, 
//             Dictionary<string, bool> featureConfigurations, 
//             IUsernameProvider usernameProvider)
//         {
//             if (features == null) throw new ArgumentNullException(nameof(features));
//
//             _featureConfigurations = featureConfigurations ?? new Dictionary<string, bool>();
//             _usernameProvider = usernameProvider ?? throw new ArgumentNullException(nameof(usernameProvider));
//
//             Initialize(features);
//         }
//
//         private Dictionary<string, bool> FeatureConfigurations => _featureConfigurations;
//
//         private void Initialize(IList<Feature> features)
//         {
//             foreach (var feature in features)
//             {
//                 FeatureConfigurations[feature.FeatureName] = feature.IsEnabled;
//             }
//         }
//
//         private void Initialize(IFeatureRepository repository)
//         {
//             var username = _usernameProvider.GetUsername();
//             var features = repository.GetFeaturesByUsername<Feature>(username);
//
//             if (!string.IsNullOrWhiteSpace(username))
//             {
//                 var userSpecificFeatures = features
//                     .Where(f => !string.IsNullOrWhiteSpace(f.Username))
//                     .ToList();
//
//                 foreach (var userFeature in userSpecificFeatures)
//                 {
//                     RemoveGenericUserFeatureConfiguration(features, userFeature);
//                 }
//             }
//
//             Initialize(features);
//         }
//
//         /// <summary>
//         /// Removes a generic (non-user-specific) feature if a user-specific one exists.
//         /// </summary>
//         private static void RemoveGenericUserFeatureConfiguration(
//             IList<Feature> features, 
//             Feature userSpecificFeature)
//         {
//             var genericFeature = features.FirstOrDefault(
//                 f => f.FeatureName == userSpecificFeature.FeatureName &&
//                      string.IsNullOrWhiteSpace(f.Username));
//
//             if (genericFeature != null)
//             {
//                 features.Remove(genericFeature);
//             }
//         }
//
//         private bool IsEnabled(string featureName, bool defaultValue)
//         {
//             return FeatureConfigurations.TryGetValue(featureName, out var enabled) ? enabled : defaultValue;
//         }
//
//         // Example features
//         public bool CustomerSatisfaction
//         {
//             get => IsEnabled("CustomerSatisfaction", false);
//             set => throw new NotImplementedException();
//         }
//
//         public bool FeatureUsageLogging => IsEnabled("FeatureUsageLogging", false);
//
//         public bool PerformanceCounters => IsEnabled("PerformanceCounters", false);
//
//         public bool Search
//         {
//             get => IsEnabled("Search", true);
//             set => throw new NotImplementedException();
//         }
//
//         public bool SearchByBirthBusinessProvince
//         {
//             get => IsEnabled("SearchByBirthBusinessProvince", false);
//             set => throw new NotImplementedException();
//         }
//     }
//
//     public interface IUsernameProvider
//     {
//         string GetUsername();
//     }
//
//
// }
