import 'package:barbermanagemobile/data/models/promotion_model.dart';
import 'package:barbermanagemobile/domain/repositories/promotion_repository.dart';

class GetPromotionsUseCase {
  final PromotionRepository repository;

  GetPromotionsUseCase(this.repository);

  Future<List<PromotionModel>> call() async {
    return await repository.getPromotions();
  }
}